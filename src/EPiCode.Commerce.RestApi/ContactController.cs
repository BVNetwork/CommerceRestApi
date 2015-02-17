using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Security;
using EPiCode.Commerce.RestService.DataObjects;
using Mediachase.BusinessFoundation.Data;
using Mediachase.BusinessFoundation.Data.Business;
using Mediachase.BusinessFoundation.Data.Meta;
using Mediachase.Commerce.Core;
using Mediachase.Commerce.Customers;
using Mediachase.Commerce.Security;
using ServiceApi.DataObjects;

namespace EPiCode.Commerce.RestService
{
    public class ContactController : SecuredApiController
    {
        /// <summary>
        /// Returns all 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CustomerContact> Get()
        {
            IEnumerable<CustomerContact> contacts = GetAllContacts();
            return contacts;
        }

        // GET api/<controller>/F3A2CF62-250E-4F26-AE6B-13067E632143
        public CustomerContact Get(Guid id)
        {
            CustomerContact contact = CustomerContext.Current.GetContactById(id);
            if (contact == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return contact;
        }

        public CustomerContact Get(string email)
        {
            try
            {
                EntityObject[] entityObjectArray = BusinessManager.List("Contact", 
                    new[]
                    {
                      new FilterElement("Email", FilterElementType.Equal, email)
                    });

                if (entityObjectArray != null)
                {
                    if (entityObjectArray.Any())
                    {
                        CustomerContact customerContact = entityObjectArray[0] as CustomerContact;
                        if (customerContact != null)
                            return customerContact;
                    }
                }
            }
            catch (ObjectNotFoundException)
            {
            }

            throw new HttpResponseException(HttpStatusCode.NotFound);
        }


        internal IEnumerable<CustomerContact> GetAllContacts()
        {
            foreach (EntityObject entityObject in BusinessManager.List("Contact", new FilterElement[0], new SortingElement[0], 0, int.MaxValue))
            {
                if (entityObject is CustomerContact)
                    yield return entityObject as CustomerContact;
            }
        }

    }
}