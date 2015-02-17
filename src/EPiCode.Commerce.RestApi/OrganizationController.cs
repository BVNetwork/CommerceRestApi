using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Mediachase.BusinessFoundation.Data;
using Mediachase.Commerce.Customers;

namespace EPiCode.Commerce.RestService
{
    public class OrganizationController : SecuredApiController
    {
        // GET api/<controller>
        public IEnumerable<DataObjects.Organization> Get()
        {
            IEnumerable<Organization> organizations = CustomerContext.Current.GetAllOrganization();

            var organizationList = from Organization c in organizations select new DataObjects.Organization(c);
            
            return organizationList;
        }

        // GET api/<controller>/F3A2CF62-250E-4F26-AE6B-13067E632143
        public DataObjects.Organization Get(Guid id)
        {
            Organization organization = CustomerContext.Current.GetOrganizationById(new PrimaryKeyId(id));
            if (organization != null)
                return new DataObjects.Organization(organization);
            return null;
        }

        // POST api/<controller>
        /// <summary>
        /// Add a new customer
        /// </summary>
        /// <param name="organization"></param>
        public DataObjects.Organization Post([FromBody] DataObjects.Organization organization)
        {

            return null;
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