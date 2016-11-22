using System;
using System.Web.Http;
using EPiServer.ServiceLocation;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Core;
using Mediachase.Commerce.Inventory;
using Mediachase.Commerce.InventoryService;
using Newtonsoft.Json.Linq;

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
        public object Get(string code)
        {
            var inventoryService = ServiceLocator.Current.GetInstance<IInventoryService>();
            //var warehouseRepository = ServiceLocator.Current.GetInstance<IWarehouseRepository>();
            //var defaultWarehouse = warehouseRepository.GetDefaultWarehouse();

            return inventoryService.QueryByEntry(new[] {code});
        }

        [HttpPost]
        public object Post(JObject inventory)
        {

            var inventoryService = ServiceLocator.Current.GetInstance<IInventoryService>();
            var warehouseRepository = ServiceLocator.Current.GetInstance<IWarehouseRepository>();
            var defaultWarehouse = warehouseRepository.GetDefaultWarehouse();


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

            var warehouse = warehouseRepository.Get(warehouseCode);
            if (warehouse == null)
                throw new ArgumentNullException("warehouse");

            decimal inStockQuantity = inventory["InStockQuantity"].Value<decimal>();
            if(inStockQuantity == 0)
            {
                throw new ArgumentNullException("InStockQuantity", "InStockQuantity is required");
            }


            var existingInventory = inventoryService.Get(code, warehouse.Code);

            InventoryRecord inv;
            if (existingInventory != null)
            {
                inv = new InventoryRecord(existingInventory);
            }
            else
            {
                inv = new InventoryRecord();
                inv.WarehouseCode = warehouse.Code;
                inv.CatalogEntryCode = code;
            }
            inv.PurchaseAvailableQuantity = inStockQuantity;

            // Set tracking status, if passed in, if not, ignore it
            string status = inventory["InventoryStatus"].Value<string>();
            if (string.IsNullOrEmpty(status) == false)
            {
                InventoryTrackingStatus inventoryTrackingStatus;
                if(Enum.TryParse(status, true, out inventoryTrackingStatus))
                {
                    inv.IsTracked = (inventoryTrackingStatus == InventoryTrackingStatus.Enabled);
                }
            }

            inventoryService.Save(new[] {inv});

            return Get(code);
        }


    }
}