using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EPiCode.Commerce.RestService.DataObjects;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Catalog.Managers;
using Mediachase.Commerce.Catalog.Objects;
using Mediachase.Commerce.Customers;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Orders.Managers;
using Mediachase.Commerce.Website.Helpers;
using Mediachase.Data.Provider;
using Mediachase.MetaDataPlus.Configurator;
using ServiceApi.DataObjects;

namespace EPiCode.Commerce.RestService
{
    /// <summary>
    /// 
    /// </summary>
    public class CartInfoController : SecuredApiController
    {
        private Guid RestApiUserGuid = new Guid("A47CB82D-8A07-45D7-A8D1-667919855387");

        /// <summary>
        /// Gets the ID of all orders
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Cart Get()
        {
            CartHelper helper = new CartHelper(Cart.DefaultName);
            return helper.Cart;
        }

        [HttpGet]
        public HttpResponseMessage ConvertToPurchaseOrder(int cartId)
        {
            Cart cart = Cart.LoadByOrderGroupId(cartId);
            if (cart == null)
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            ConvertCartToPurchaseOrder(cart);
            return new HttpResponseMessage(HttpStatusCode.OK);

        }

        protected void ConvertCartToPurchaseOrder(OrderGroup order)
        {
            Cart cart = order as Cart;
            if (cart != null)
            {
                using (TransactionScope transactionScope = new TransactionScope())
                {
                    OrderGroupWorkflowManager.RunWorkflow(cart, "CartPrepare");
                    PurchaseOrder purchaseOrder = cart.SaveAsPurchaseOrder();
                    if (purchaseOrder != null)
                    {
                        SetNonNullableFields(purchaseOrder);

                        string userInfo = "Not Authenticated";
                        if(User.Identity.IsAuthenticated)
                        {
                            userInfo = User.Identity.Name;
                        }

                        string note = string.Format("Created order based on cart from Commerce Rest API. User: {0}", userInfo);
                        OrderNotesManager.AddNoteToPurchaseOrder(purchaseOrder, note, OrderNoteTypes.Custom, RestApiUserGuid);
                        OrderStatusManager.RecalculatePurchaseOrder(purchaseOrder);
                        purchaseOrder.AcceptChanges();

                        // After we have successfully created the order, we delete the cart
                        cart.Delete();
                        cart.AcceptChanges();
                    }
                    transactionScope.Complete();
                }
            }
        }

        /// <summary>
        /// Pre 9.24, a non nullable bool field on the orderform would prevent us from creating it, so we set all
        /// the values to false by default if the existing value is null.
        /// </summary>
        /// <param name="purchaseOrder"></param>
        protected void SetNonNullableFields(PurchaseOrder purchaseOrder)
        {
            foreach (var field in purchaseOrder.MetaClass.MetaFields)
            {
                if (field.IsUser && field.AllowNulls == false && field.DataType == MetaDataType.Boolean)
                {
                    if (purchaseOrder[field.Name] == null)
                    {
                        purchaseOrder.SetMetaField(field.Name, false, true);
                    }
                }
            }
        }



        // POST api/<controller>
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

    }
}