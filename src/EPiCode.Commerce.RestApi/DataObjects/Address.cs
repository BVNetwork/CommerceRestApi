using Mediachase.Commerce.Customers;

namespace EPiCode.Commerce.RestService.DataObjects
{
    public class Address
    {
        public Address()
        {
        }

        public Address(CustomerAddress customerAddress)
        {
            City = customerAddress.City;
            CountryCode = customerAddress.CountryCode;
            CountryName = customerAddress.CountryName;
            Email = customerAddress.Email;
            FirstName = customerAddress.FirstName;
            LastName = customerAddress.LastName;
            Name = customerAddress.Name;
            PostalCode = customerAddress.PostalCode;
            RegionCode = customerAddress.RegionCode;
            RegionName = customerAddress.RegionName;
            State = customerAddress.State;
            DaytimePhoneNumber = customerAddress.DaytimePhoneNumber;
            EveningPhoneNumber = customerAddress.EveningPhoneNumber;
            Line1 = customerAddress.Line1;
            Line2 = customerAddress.Line2;
        }

        public void Populate(CustomerAddress customerAddress)
        {
            customerAddress.City = City;
            customerAddress.CountryCode = CountryCode;
            customerAddress.CountryName = CountryName;
            customerAddress.Email = Email;
            customerAddress.FirstName = FirstName;
            customerAddress.LastName = LastName;
            customerAddress.Name = Name;
            customerAddress.PostalCode = PostalCode;
            customerAddress.RegionCode = RegionCode;
            customerAddress.RegionName = RegionName;
            customerAddress.State = State;
            customerAddress.DaytimePhoneNumber = DaytimePhoneNumber;
            customerAddress.EveningPhoneNumber = EveningPhoneNumber;
            customerAddress.Line1 = Line1;
            customerAddress.Line2 = Line2;
        }

        public string City { get; set; }
        public string EveningPhoneNumber { get; set; }
        public string DaytimePhoneNumber { get; set; }
        public string State { get; set; }
        public string RegionName { get; set; }
        public string RegionCode { get; set; }
        public string PostalCode { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
    }
}