using System;
using System.Collections.Generic;
using Mediachase.BusinessFoundation.Common;
using Mediachase.Commerce.Customers;

namespace EPiCode.Commerce.RestService.DataObjects
{
    public class Customer
    {
        public Customer()
        {
            Addresses = new List<Address>();
        }

        public Customer(CustomerContact customerContact) : this()
        {
            Code = customerContact.Code;
            FullName = customerContact.FullName;
            Email = customerContact.Email;
            FirstName = customerContact.FirstName;
            MiddleName = customerContact.MiddleName;
            LastName = customerContact.LastName;
            LastOrder = customerContact.LastOrder;
            Owner = customerContact.Owner;
            OwnerId = customerContact.OwnerId != null ? customerContact.OwnerId.ToString() : null;
            PreferredLanguage = customerContact.PreferredLanguage;
            PreferredCurrency = customerContact.PreferredCurrency;
            PrimaryKeyId = customerContact.PrimaryKeyId != null ? customerContact.PrimaryKeyId.ToString() : null;
            ProviderUserKey = customerContact.ProviderUserKey;
            RegistrationSource = customerContact.RegistrationSource;
            UserId = customerContact.UserId;
            BirthDate = customerContact.BirthDate;

            if (customerContact["Gender"] != null)
            {
                Gender = EntityObjectHelper.GetStringEntityEnumProperyValue(customerContact, "Gender");
            }

            foreach (CustomerAddress customerAddress in customerContact.ContactAddresses)
            {
                Address address = new Address(customerAddress);
                Addresses.Add(address);
            }
        }

        public string PrimaryKeyId { get; set; }
        public string OwnerId { get; set; }
        public string PreferredCurrency { get; set; }
        public string PreferredLanguage { get; set; }
        public string Owner { get; set; }
        public DateTime? LastOrder { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public object ProviderUserKey { get; set; }
        public string RegistrationSource { get; set; }
        public string UserId { get; set; }
        public DateTime? BirthDate { get; set; }
        public string FullName { get; set; }
        public string Code { get; set; }
        public string UserName { get; set; }
        public string Gender { get; set; }
        public List<Address> Addresses { get; set; }
        
    }
}