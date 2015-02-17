using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Catalog.Dto;
using Mediachase.Commerce.Catalog.Managers;

namespace EPiCode.Commerce.RestService
{
    public class ProductController : SecuredApiController
    {
        [HttpGet]
        public CatalogEntryDto Get(int productId)
        {
            ICatalogSystem catalog = EPiServer.ServiceLocation.ServiceLocator.Current.GetInstance<ICatalogSystem>();
            CatalogEntryResponseGroup responseGroup = new CatalogEntryResponseGroup(CatalogEntryResponseGroup.ResponseGroup.CatalogEntryFull);
            CatalogEntryDto entryDto = catalog.GetCatalogEntryDto(productId, responseGroup);
            if (entryDto == null || entryDto.CatalogEntry.Rows.Count == 0)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            
            return entryDto;
        }

        /// <summary>
        /// Gets the Catalog Entry Dto based on the Product Code
        /// </summary>
        /// <param name="productCode"></param>
        /// <returns></returns>
        [HttpGet]
        public CatalogEntryDto Get(string productCode)
        {
            ICatalogSystem catalog = EPiServer.ServiceLocation.ServiceLocator.Current.GetInstance<ICatalogSystem>();
            CatalogEntryResponseGroup responseGroup = new CatalogEntryResponseGroup(CatalogEntryResponseGroup.ResponseGroup.CatalogEntryFull);
            CatalogEntryDto entryDto = catalog.GetCatalogEntryDto(productCode, responseGroup);
            if (entryDto == null || entryDto.CatalogEntry.Rows.Count == 0)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            
            return entryDto;
        }

        [HttpGet]
        public IEnumerable<CatalogEntryDto.VariationRow> GetVariations(int productId)
        {
            ICatalogSystem catalog = EPiServer.ServiceLocation.ServiceLocator.Current.GetInstance<ICatalogSystem>();
            CatalogEntryResponseGroup responseGroup = new CatalogEntryResponseGroup(CatalogEntryResponseGroup.ResponseGroup.CatalogEntryFull);
            CatalogEntryDto variationEntries = catalog.GetCatalogEntriesDto(productId, 
                string.Empty, 
                string.Empty, 
                responseGroup);
            return variationEntries.Variation.ToList();
        }

        [HttpGet]
        public IEnumerable<CatalogEntryDto.VariationRow> GetVariations(string productCode)
        {
            ICatalogSystem catalog = EPiServer.ServiceLocation.ServiceLocator.Current.GetInstance<ICatalogSystem>();

            CatalogEntryDto product = Get(productCode);
            
            if (product.CatalogEntry != null && product.CatalogEntry.Rows.Count > 0)
            {
                CatalogEntryResponseGroup responseGroup = new CatalogEntryResponseGroup(CatalogEntryResponseGroup.ResponseGroup.CatalogEntryFull); 
                int productId = product.CatalogEntry[0].CatalogEntryId;
                // Need to call this overload due to a bug, which I cannot recall at the moment
                CatalogEntryDto variationEntries = catalog.GetCatalogEntriesDto(productId,
                    string.Empty,
                    string.Empty,
                    responseGroup);

                // variationEntries.Variation[0]

                return variationEntries.Variation.ToList();
                
            }
            return null;
        }


        [HttpGet]
        public int GetProductIdFromCode(string code)
        {
            ICatalogSystem catalog = EPiServer.ServiceLocation.ServiceLocator.Current.GetInstance<ICatalogSystem>();
            CatalogEntryResponseGroup responseGroup = new CatalogEntryResponseGroup(CatalogEntryResponseGroup.ResponseGroup.CatalogEntryInfo);
            CatalogEntryDto entryDto = catalog.GetCatalogEntryDto(code, responseGroup);
            if(entryDto == null || entryDto.CatalogEntry.Rows.Count == 0)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            
            return entryDto.CatalogEntry[0].CatalogEntryId;
        }

        [HttpGet]
        public EntryContentBase GetProductContentByCode(string productCode)
        {
            IContentRepository repository = EPiServer.ServiceLocation.ServiceLocator.Current.GetInstance<IContentRepository>();
            ReferenceConverter referenceConverter = EPiServer.ServiceLocation.ServiceLocator.Current.GetInstance<ReferenceConverter>();
            ContentReference link = referenceConverter.GetContentLink(productCode);
            EntryContentBase contentBase = repository.Get<EPiServer.Commerce.Catalog.ContentTypes.EntryContentBase>(link);
            return contentBase;
        }
    }
}