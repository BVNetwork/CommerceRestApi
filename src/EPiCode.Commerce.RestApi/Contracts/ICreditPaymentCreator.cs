using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPiServer.ServiceLocation;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Orders.Dto;
using Mediachase.Commerce.Orders.Managers;

namespace EPiCode.Commerce.RestService.Contracts
{
    public interface ICreditPaymentCreator
    {
        T CreateCreditPayment<T>(OrderForm returnForm, OrderGroup order, Guid paymentMethodId) where T: OtherPayment;
    }

    [ServiceConfiguration(typeof(ICreditPaymentCreator))]
    public class CreditPaymentCreator : ICreditPaymentCreator {


        public T CreateCreditPayment<T>(OrderForm returnForm, OrderGroup order, Guid paymentMethodId) where T : OtherPayment
        {
            OtherPayment otherPayment = new OtherPayment
            {
                PaymentMethodId = paymentMethodId,
                PaymentMethodName = ((PaymentMethodDto.PaymentMethodRow)PaymentManager.GetPaymentMethod(paymentMethodId).PaymentMethod.Rows[0]).Name,
                BillingAddressId = returnForm.BillingAddressId,
                TransactionType = TransactionType.Credit.ToString(),
                Amount = returnForm.Total
            };

            returnForm.Payments.Add(otherPayment);

            return (T)otherPayment;
        }

    }
}
