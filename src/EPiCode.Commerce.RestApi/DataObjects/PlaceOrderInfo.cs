using System;
using System.Collections.Generic;

namespace EPiCode.Commerce.RestService.DataObjects
{
    public class PlaceOrderInfo
    {
        public PlaceOrderInfo()
        {
            OrderDate = DateTime.Now;
            Products = new List<PlaceOrderLineItemInfo>();
        }

        public Guid CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public List<PlaceOrderLineItemInfo> Products { get; set; }
    }

    public class PlaceOrderLineItemInfo
    {
        public PlaceOrderLineItemInfo()
        {
            Count = 1;
        }

        public int Count { get; set; }
        public string ProductEntryCode { get; set; }
        public string VariationEntryCode { get; set; }
        
    }
}