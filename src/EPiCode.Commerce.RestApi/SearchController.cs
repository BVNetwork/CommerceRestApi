using System.Collections.Generic;
using System.Web.Http;
using System.Web.Security;
using Mediachase.Commerce.Core;
using Mediachase.Commerce.Core.Dto;
using Mediachase.Search;

namespace EPiCode.Commerce.RestService
{
    public class SearchController : SecuredApiController
    {
        [HttpGet]
        [Authorize(Roles = "CmsAdmins")]
        public int Reindex()
        {
            foreach (AppDto.ApplicationRow application in AppContext.Current.GetApplicationDto().Application)
            {
                SearchManager manager = new SearchManager(application.Name);
                manager.SearchIndexMessage += delegate(object source, SearchIndexEventArgs args)
                    {
                        System.Diagnostics.Debug.WriteLine(args.CompletedPercentage + ": " + args.Message);
                    };
                manager.BuildIndex(rebuild: true);
            }

            return 0;
        }


        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}