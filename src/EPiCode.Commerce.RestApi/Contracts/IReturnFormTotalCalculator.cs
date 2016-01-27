using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPiServer.ServiceLocation;
using Mediachase.BusinessFoundation.Blob;
using Mediachase.Commerce.Orders;

namespace EPiCode.Commerce.RestService.Contracts
{
    public interface IReturnFormTotalCalculator
    {
        void AdjustReturnTotal(OrderForm returnForm);
    }

    [ServiceConfiguration(typeof(IReturnFormTotalCalculator))]
    public class ReturnFormTotalCalculator : IReturnFormTotalCalculator
    {

        public void AdjustReturnTotal(OrderForm returnForm)
        {
            //Add items or change shipment
        }
    }
}
