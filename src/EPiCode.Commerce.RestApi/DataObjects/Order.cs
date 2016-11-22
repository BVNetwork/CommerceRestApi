using System.Collections.Generic;
using Mediachase.Commerce.Orders;

namespace EPiCode.Commerce.RestService.DataObjects
{
    public class Order
    {
        public Order()
        {
            LineItems = new List<OrderLineItem>();
        }
        public int OrderId { get; set; }
        public List<OrderLineItem> LineItems { get; set; }

        public void PopulateFrom(OrderForm orderForm)
        {
            OrderId = orderForm.Id;

            // Something
            foreach (LineItem item in orderForm.LineItems)
            {
                OrderLineItem orderLineItem = new OrderLineItem()
                {
                    ParentCatalogEntryId = item.ParentCatalogEntryId,
                    Code = item.Code,
                    Name = item.DisplayName
                };
                LineItems.Add(orderLineItem);
            }

        }

    }

    public class OrderLineItem
    {
        public string ParentCatalogEntryId { get; set; }
        public string CatalogEntryId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}