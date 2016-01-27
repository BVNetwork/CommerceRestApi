using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPiServer.ServiceLocation;
using Mediachase.Commerce.Orders;

namespace EPiCode.Commerce.RestService.Contracts
{
    public interface IOrderFormPaymentRetriever
    {
        Payment GetCapturedPayment(OrderGroup order);
    }

    [ServiceConfiguration(typeof(IOrderFormPaymentRetriever))]
    public class OrderFormPaymentRetriever : IOrderFormPaymentRetriever
    {

        public Payment GetCapturedPayment(OrderGroup order)
        {
            return order.OrderForms[0].Payments.ToArray().FirstOrDefault(x => x.Status == PaymentStatus.Processed.ToString() && IsCorrectType(x));
        }

        private bool IsCorrectType(Payment payment)
        {
            string transactionType = payment.TransactionType;

            return transactionType == TransactionType.Capture.ToString() ||
                   transactionType == TransactionType.Sale.ToString();

        }
    }
}
