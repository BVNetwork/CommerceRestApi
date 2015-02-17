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
using ServiceApi.DataObjects;

namespace EPiCode.Commerce.RestService
{
    /// <summary>
    /// 
    /// </summary>
    public class CartInfoController : SecuredApiController
    {

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