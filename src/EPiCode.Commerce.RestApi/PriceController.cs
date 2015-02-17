using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EPiCode.Commerce.RestService.DataObjects;
using EPiServer.ServiceLocation;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Catalog.Managers;
using Mediachase.Commerce.Catalog.Objects;
using Mediachase.Commerce.Core;
using Mediachase.Commerce.Customers;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Orders.Managers;
using Mediachase.Commerce.Pricing;
using Mediachase.Commerce.Website.Helpers;
using Newtonsoft.Json.Linq;
using ServiceApi.DataObjects;

namespace EPiCode.Commerce.RestService
{
    /// <summary>
    /// 
    /// </summary>
    public class PriceController : SecuredApiController
    {

        /// <summary>
        /// Gets price information about one variation
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object Get(string code)
        {
            CatalogKey key = new CatalogKey(AppContext.Current.ApplicationId, code);

            IPriceService priceService = ServiceLocator.Current.GetInstance<IPriceService>();
            IPriceDetailService priceDetailService = ServiceLocator.Current.GetInstance<IPriceDetailService>();

            IEnumerable<IPriceValue> catalogEntryPrices = priceService.GetCatalogEntryPrices(key);
            return catalogEntryPrices;
        }

        [HttpGet]
        public object GetPriceTypes()
        {
            PriceTypeConfiguration priceTypeConfiguration = ServiceLocator.Current.GetInstance<PriceTypeConfiguration>();
            return priceTypeConfiguration.PriceTypeDefinitions;
        }
    }
}