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
using Mediachase.Commerce.Inventory;
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
    public class InventoryController : SecuredApiController
    {

        /// <summary>
        /// Gets price information about one variation
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object Get(string id)
        {
            CatalogKey key = new CatalogKey(AppContext.Current.ApplicationId, id);

            var warehouseInventoryService = ServiceLocator.Current.GetInstance<IWarehouseInventoryService>();

            return warehouseInventoryService.List(key);
        }

        [HttpPost]
        public object Post(JObject inventory)
        {

            // Need the catalog code
            string code = inventory["CatalogEntryCode"].Value<string>();
            if(string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentNullException("CatalogEntryCode", "CatalogEntryCode is required");
            }

            string warehouseCode = "default";
            if(string.IsNullOrEmpty(inventory["WarehouseCode"].Value<string>()) == false)
            {
                warehouseCode = inventory["WarehouseCode"].Value<string>();
            }
            IWarehouseRepository warehouseRepository = ServiceLocator.Current.GetInstance<IWarehouseRepository>();
            var warehouse = warehouseRepository.Get(warehouseCode);
            if (warehouse == null)
                throw new ArgumentNullException("warehouse");

            decimal inStockQuantity = inventory["InStockQuantity"].Value<decimal>();
            if(inStockQuantity == 0)
            {
                throw new ArgumentNullException("InStockQuantity", "InStockQuantity is required");
            }

            CatalogKey key = new CatalogKey(AppContext.Current.ApplicationId, code);

            var inventoryService = ServiceLocator.Current.GetInstance<IWarehouseInventoryService>();

            var existingInventory = inventoryService.Get(key, warehouse);

            WarehouseInventory inv;
            if (existingInventory != null)
            {
                inv = new WarehouseInventory(existingInventory);
            }
            else
            {
                inv = new WarehouseInventory();
                inv.WarehouseCode = warehouse.Code;
                inv.CatalogKey = key;
            }
            inv.InStockQuantity = inStockQuantity;

            // Set tracking status, if passed in, if not, ignore it
            string status = inventory["InventoryStatus"].Value<string>();
            if (string.IsNullOrEmpty(status) == false)
            {
                InventoryTrackingStatus inventoryTrackingStatus;
                if(Enum.TryParse(status, true, out inventoryTrackingStatus))
                {
                    inv.InventoryStatus = inventoryTrackingStatus;
                }
            }

            inventoryService.Save(inv);

            return Get(key.CatalogEntryCode);
        }


    }
}