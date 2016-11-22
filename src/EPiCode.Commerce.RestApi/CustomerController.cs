using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Security;
using EPiCode.Commerce.RestService.DataObjects;
using Mediachase.BusinessFoundation.Data;
using Mediachase.BusinessFoundation.Data.Business;
using Mediachase.Commerce.Core;
using Mediachase.Commerce.Customers;
using Mediachase.Commerce.Security;
using ServiceApi.DataObjects;

namespace EPiCode.Commerce.RestService
{
    public class CustomerController : SecuredApiController
    {
        // GET api/<controller>
        public IEnumerable<Customer> Get()
        {
            IEnumerable<CustomerContact> contacts = GetAllContacts();

            var customerList = from CustomerContact c in contacts select new Customer(c);

            //return mciList.ToList();
            return customerList;
        }

        // GET api/<controller>/F3A2CF62-250E-4F26-AE6B-13067E632143
        public Customer Get(Guid id)
        {

            CustomerContact contact = CustomerContext.Current.GetContactById(id);
            if (contact != null)
                return new Customer(contact);

            return null;
        }

        // POST api/<controller>
        /// <summary>
        /// Add a new customer
        /// </summary>
        /// <param name="customer"></param>
        public Customer Post([FromBody] Customer customer)
        {
            CustomerContact newContact = CreateCustomerContact(customer);
            
            // Fetch contact from db
            Customer createdCustomer = Get((Guid)newContact.PrimaryKeyId);

            return createdCustomer;
        }

        [HttpPost]
        public Guid AddAddressToCustomer([FromUri] Guid customerId, [FromBody] Address address)
        {
            CustomerContact contact = CustomerContext.Current.GetContactById(customerId);
            CustomerAddress newAddress = CustomerAddress.CreateForApplication(AppContext.Current.ApplicationId);

            // Get data from contact
            if(string.IsNullOrEmpty(address.FirstName))
            {
                address.FirstName = contact.FirstName;
            }
            if (string.IsNullOrEmpty(address.LastName))
            {
                address.LastName = contact.LastName;
            }
            if (string.IsNullOrEmpty(address.Email))
            {
                address.Email = contact.Email;
            }

            address.Populate(newAddress);
            contact.AddContactAddress(newAddress);
            contact.SaveChanges();

            return (Guid)newAddress.AddressId;

        }

        private CustomerContact CreateCustomerContact(Customer customer)
        {

            // Now create an account in the ECF
            CustomerContact customerContact = CustomerContact.CreateInstance();
            customerContact.FirstName = customer.FirstName;
            customerContact.LastName = customer.LastName;
            customerContact.RegistrationSource = customer.RegistrationSource;
            customerContact.Email = customer.Email;
            customerContact.FullName = customer.FirstName + " " + customer.LastName;
            customerContact.BirthDate = customer.BirthDate;
            // customerContact.CustomerGroup
            customerContact.SaveChanges();
            return customerContact;

        }


        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
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