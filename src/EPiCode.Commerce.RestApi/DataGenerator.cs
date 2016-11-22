using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Http;
using EPiCode.Commerce.RestService.DataObjects;
using EPiServer.Logging;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Catalog.Dto;
using Mediachase.Commerce.Catalog.Managers;
using Mediachase.Commerce.Catalog.Search;
using Mediachase.Commerce.Customers;
using ServiceApi;
using ServiceApi.DataObjects;

namespace EPiCode.Commerce.RestService
{
    public class DataGeneratorController : SecuredApiController
    {
        protected static ILogger _log = LogManager.GetLogger();

        protected readonly string[] _registrationSources = new string[]
            {
                "Facebook",
                "Website",
                "CustomerService",
                "CrmSystem"
            };

        //[HttpGet]
        //public List<Address> CreateCustomers(int count)
        //{
        //    Stopwatch tmr = Stopwatch.StartNew();
        //    _log.Debug("Start Creating {0} customers", count);

        //    CustomerController cc = new CustomerController();
        //    Generator generator = new Generator();
        //    string basePath = HostingEnvironment.MapPath("~/App_Data/datagenerator");

        //    Random rnd = new Random(DateTime.Now.Millisecond);

        //    List<Address> addresses = generator.GenerateAddresses(count, basePath);
        //    foreach (Address address in addresses)
        //    {
        //        Customer cust = new Customer
        //            {
        //                BirthDate = address.BirthDate,
        //                Email = address.Email,
        //                FirstName = address.FirstName,
        //                LastName = address.LastName,
        //                // Random registration source
        //                RegistrationSource = _registrationSources[rnd.Next(0, _registrationSources.Length)],
        //                UserName = address.Email
        //            };

        //        Customer customer = cc.Post(cust);
        //        Guid custId = new Guid(customer.PrimaryKeyId);

        //        DataObjects.Address newAddress = new DataObjects.Address
        //            {
        //                Name = "Home",
        //                FirstName = cust.FirstName,
        //                LastName = cust.LastName,
        //                City = address.City,
        //                CountryCode = "NOR",
        //                CountryName = "Norway",
        //                Email = address.Email,
        //                Line1 = address.StreetAddress,
        //                PostalCode = address.PostalCode,
        //                RegionName = address.Municipality,
        //                State = address.County
        //            };

        //        cc.AddAddressToCustomer(custId, newAddress);
        //    }

        //    tmr.Stop();
        //    _log.Debug("Created {0} customers in {1}ms", count, tmr.ElapsedMilliseconds);
        //    return addresses;
        //}

        [HttpPost]
        public string CreateOrders([FromBody] OrderGeneratorSettings settings)
        {
            Stopwatch tmr = Stopwatch.StartNew();
            _log.Debug("Start Creating {0} orders", settings.NumberOfOrdersToGenerate);

            // We need all Products with variations
            ICatalogSystem catalog = EPiServer.ServiceLocation.ServiceLocator.Current.GetInstance<ICatalogSystem>();
            Dictionary<int, CatalogEntryDto.CatalogEntryRow> catalogEntryRows = new Dictionary<int, CatalogEntryDto.CatalogEntryRow>();
            WalkCatalog(catalog, catalogEntryRows);

            long elapsedMilliseconds = tmr.ElapsedMilliseconds;
            _log.Debug("Reading {0} entries took {1}ms", catalogEntryRows.Count, elapsedMilliseconds);

            // Build the relationship between the products and their variations
            List<ProductInfo> productsAndVariations = BuildProductVariationRelations(catalogEntryRows, catalog);
            _log.Debug("There are {0} products (with variations)", productsAndVariations.Count);            

            // Load All customers, we need to distribute the orders among them
            CustomerController cc = new CustomerController();
            List<Customer> customers = cc.Get().ToList();
            _log.Debug("There are {0} customers to distribute {1} orders on", productsAndVariations.Count, customers.Count);
            
            // Create them orders - this is where the bulk of the job is taking place
            string returnInfo = CreateOrdersForCustomers(settings, productsAndVariations, customers);

            tmr.Stop();
            return returnInfo;
        }

        [HttpGet]
        public string UpdateAllCustomerData()
        {
            string[] genders = new string[]
                {
                    "Male",
                    "Female",
                    "ItsComplicated"
                };
 
            Random rnd = new Random(DateTime.Now.Millisecond);

            IEnumerable<CustomerContact> contacts = new CustomerController().GetAllContacts();

            foreach (CustomerContact contact in contacts)
            {
                contact.BirthDate = new DateTime(rnd.Next(1955, 2000), rnd.Next(1, 12), rnd.Next(1, 28));
                contact["Gender"] = rnd.Next(1, _registrationSources.Length);
                contact.RegistrationSource = _registrationSources[rnd.Next(0, _registrationSources.Length)];
                contact.SaveChanges();
            }

            return string.Format("Updated {0}", contacts.Count());
        }

        private List<ProductInfo> BuildProductVariationRelations(Dictionary<int, CatalogEntryDto.CatalogEntryRow> catalogEntryRows, ICatalogSystem catalog)
        {
            List<ProductInfo> productsAndVariations = new List<ProductInfo>();
            CatalogEntryResponseGroup variatonRespGroup =
                new CatalogEntryResponseGroup(CatalogEntryResponseGroup.ResponseGroup.CatalogEntryInfo);

            // Build product -> variation hierarchy
            foreach (KeyValuePair<int, CatalogEntryDto.CatalogEntryRow> keyValue in catalogEntryRows)
            {
                CatalogEntryDto.CatalogEntryRow entryRow = keyValue.Value;
                if (string.Compare(entryRow.ClassTypeId, "Product", StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    ProductInfo productInfo = new ProductInfo
                        {
                            EntryRow = entryRow,
                            CatalogEntryId = keyValue.Key,
                            Code = entryRow.Code,
                            Name = entryRow.Name
                        };

                    // Now load all related variations manually
                    CatalogEntryDto variationEntries = catalog.GetCatalogEntriesDto(keyValue.Key, string.Empty, string.Empty,
                                                                                    variatonRespGroup);

                    foreach (CatalogEntryDto.CatalogEntryRow variationRow in variationEntries.CatalogEntry)
                    {
                        if (catalogEntryRows.ContainsKey(variationRow.CatalogEntryId))
                        {
                            productInfo.Variations.Add(catalogEntryRows[variationRow.CatalogEntryId]);
                        }
                    }

                    // Only add if we have a product with one or more variations
                    if (productInfo.Variations.Count > 0)
                    {
                        productsAndVariations.Add(productInfo);
                    }
                }
            }
            return productsAndVariations;
        }

        protected void WalkCatalog(ICatalogSystem catalogSystem, Dictionary<int, CatalogEntryDto.CatalogEntryRow> catalogEntryRows)
        {
            // Get all catalogs
            CatalogDto catalogs = catalogSystem.GetCatalogDto();
            foreach (CatalogDto.CatalogRow catalog in catalogs.Catalog)
            {
                // string catalogName = catalog.Name;
                int catalogId = catalog.CatalogId;
                // Get Catalog Nodes
                CatalogNodeDto nodes = catalogSystem.GetCatalogNodesDto(catalogId);
                WalkCatalogNodes(catalogSystem, nodes, catalog, catalogEntryRows);
            }
        }

        private void WalkCatalogNodes(ICatalogSystem catalogSystem, CatalogNodeDto nodes, CatalogDto.CatalogRow catalog, Dictionary<int, CatalogEntryDto.CatalogEntryRow> catalogEntryRows)
        {
            foreach (CatalogNodeDto.CatalogNodeRow node in nodes.CatalogNode)
            {
                CatalogSearchParameters pars = new CatalogSearchParameters();
                CatalogSearchOptions options = new CatalogSearchOptions { CacheResults = false };
                pars.CatalogNames.Add(catalog.Name);
                pars.CatalogNodes.Add(node.Code);
                //CatalogEntryDto entries = CatalogContext.Current.FindItemsDto(
                //    pars,
                //    options,
                //    ref count,
                //    new CatalogEntryResponseGroup(CatalogEntryResponseGroup.ResponseGroup.CatalogEntryFull));
                CatalogEntryDto entries = catalogSystem.GetCatalogEntriesDto(catalog.CatalogId, node.CatalogNodeId,
                                                                             new CatalogEntryResponseGroup(
                                                                                 CatalogEntryResponseGroup.ResponseGroup.CatalogEntryInfo));

                _log.Debug("Entries in Node: {0} (Count: {1})", node.Name, entries.CatalogEntry.Rows.Count);
                foreach (CatalogEntryDto.CatalogEntryRow entry in entries.CatalogEntry)
                {
                    // _log.Debug("{3}: {0} ({1} - {2})", entry.Name, entry.Code, entry.CatalogEntryId, entry.ClassTypeId);
                    if (catalogEntryRows.ContainsKey(entry.CatalogEntryId) == false)
                    {
                        catalogEntryRows.Add(entry.CatalogEntryId, entry);
                    }
                }

                // Get Subnodes
                CatalogNodeDto subNodes = catalogSystem.GetCatalogNodesDto(catalog.CatalogId, node.CatalogNodeId);
                WalkCatalogNodes(catalogSystem, subNodes, catalog, catalogEntryRows);
            }
        }

        protected string CreateOrdersForCustomers(OrderGeneratorSettings settings, List<ProductInfo> productsAndVariations, List<Customer> customers)
        {
            Stopwatch tmr = Stopwatch.StartNew();

            Random rnd = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < settings.NumberOfOrdersToGenerate; i++)
            {
                // First get customer
                int randomCustomerIndex = rnd.Next(0, customers.Count - 1);
                Customer customer = customers[randomCustomerIndex];

                int dayNumber = rnd.Next(1, DateTime.Now.DayOfYear);
                DateTime orderDate = new DateTime(DateTime.Now.Year, 1, 1).AddDays(dayNumber).AddHours(rnd.Next(1, 23)).AddMinutes(rnd.Next(0,59));
                PlaceOrderInfo orderInfo = new PlaceOrderInfo
                    {
                        CustomerId = Guid.Parse(customer.PrimaryKeyId),
                        OrderDate = orderDate
                    };

                // How many line items can we add?
                int numLineItems = rnd.Next(1, settings.MaximumLineItemsPerOrder);
                for (int j = 0; j < numLineItems; j++)
                {
                    int next = rnd.Next(0, productsAndVariations.Count);
                    ProductInfo productInfo = productsAndVariations[next];
                    PlaceOrderLineItemInfo orderLineItem = new PlaceOrderLineItemInfo
                        {
                            Count = 1,
                            ProductEntryCode = productInfo.Code,
                            VariationEntryCode = productInfo.Variations[rnd.Next(0, productInfo.Variations.Count - 1)].Code
                        };
                    orderInfo.Products.Add(orderLineItem);
                }

                OrderController oc = new OrderController();
                int orderNumber = oc.CreateOrder(orderInfo);
                _log.Debug("Order for {0} on {1}: {2} products. Ordernumber: {3}",
                                 customer.FullName, orderInfo.OrderDate, orderInfo.Products.Count, orderNumber);

            }
            tmr.Stop();

            string info = string.Format("Created {0} orders for random customers in {1}", settings.NumberOfOrdersToGenerate, tmr.Elapsed);
            _log.Debug(info);
            return info;

        }

    }

    public class ProductInfo
    {
        public ProductInfo()
        {
            Variations = new List<CatalogEntryDto.CatalogEntryRow>();
        }
        public int CatalogEntryId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public CatalogEntryDto.CatalogEntryRow EntryRow { get; set; }
        public List<CatalogEntryDto.CatalogEntryRow> Variations { get; set; }
    }

}