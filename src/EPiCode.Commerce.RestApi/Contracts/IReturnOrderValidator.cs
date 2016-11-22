using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EPiCode.Commerce.RestService.DataObjects;
using EPiServer.ServiceLocation;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Orders.Managers;

namespace EPiCode.Commerce.RestService.Contracts
{
    public interface IReturnOrderValidator
    {
        bool TryValidateReturn(PurchaseOrder order, ReturnInfo returnInfo, out string[] messages);
    }

    [ServiceConfiguration(typeof (IReturnOrderValidator))]
    public class ReturnOrderValidator : IReturnOrderValidator
    {
        public bool TryValidateReturn(PurchaseOrder order, ReturnInfo returnInfo, out string[] messages)
        {
            bool isValid = true;
            var errors = new List<string>();
            if (order.ReturnOrderForms.ToArray().Any(f => f.Status == ReturnFormStatus.AwaitingStockReturn.ToString() || f.Status == ReturnFormStatus.AwaitingCompletion.ToString()))
            {
                errors.Add(string.Format("Order {0} has pending returns", order.TrackingNumber));
            }

            var returnableLineItems =
                ReturnExchangeManager.GetAvailableForReturnLineItems(order.OrderForms[0].Shipments[0]).ToList();

            foreach (ReturnItem returnItem in returnInfo.ReturnItems)
            {
                if (returnableLineItems.All(x => x.Code != returnItem.Sku))
                {
                    errors.Add(string.Format("Lineitem with code: {0} is not returnable", returnItem.Sku));
                }
            }

            messages = errors.ToArray();

            if (errors.Any())
            {
                isValid = false;
            }

            return isValid;
        }
    }
}
