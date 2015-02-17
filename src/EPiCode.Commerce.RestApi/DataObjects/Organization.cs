using System;

namespace EPiCode.Commerce.RestService.DataObjects
{
    public class Organization
    {
        public Organization()
        {
        }
        public Organization(Mediachase.Commerce.Customers.Organization organization)
        {
            BusinessCategory = organization.BusinessCategory;
            Created = organization.Created;
            Description = organization.Description;
            Modified = organization.Modified;
            Name = organization.Name;
            OrgCustomerGroup = organization.OrgCustomerGroup;
            OrganizationType = organization.OrganizationType;
            PrimaryContact = organization.PrimaryContact;
            PrimaryContactId = organization.PrimaryContactId != null ? organization.PrimaryContactId.ToString() : null;
            PrimaryKeyId = organization.PrimaryKeyId != null ? organization.PrimaryKeyId.ToString() : null;
            
        }

        public string BusinessCategory { get; set; }
        public DateTime Created { get; set; }
        public string Description { get; set; }
        public DateTime Modified { get; set; }
        public string Name { get; set; }
        public string OrgCustomerGroup { get; set; }
        public string OrganizationType { get; set; }
        public string PrimaryContact { get; set; }
        public string PrimaryContactId { get; set; }
        public string PrimaryKeyId { get; set; }
    }
}