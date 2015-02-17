using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using EPiCode.Commerce.RestService.DataObjects;
using Mediachase.MetaDataPlus;
using Mediachase.MetaDataPlus.Configurator;
using Newtonsoft.Json;
using MetaClass = EPiCode.Commerce.RestService.DataObjects.MetaClass;
using MetaField = Mediachase.MetaDataPlus.Configurator.MetaField;

namespace EPiCode.Commerce.RestService
{
    public class MetaClassController : SecuredApiController
    {
        // GET api/<controller>
        public IEnumerable<MetaClass> Get()
        {
            MetaClassCollection classCollection = Mediachase.MetaDataPlus.Configurator.MetaClass.GetList(MetaDataContext.Instance);
            var mciList = from Mediachase.MetaDataPlus.Configurator.MetaClass mc in classCollection
                          select new MetaClass(mc);

            return mciList.ToList();
        }

        public IEnumerable<MetaClass> Get(string @namespace)
        {
            MetaClassCollection classCollection = Mediachase.MetaDataPlus.Configurator.MetaClass.GetList(MetaDataContext.Instance);
            var mciList = from Mediachase.MetaDataPlus.Configurator.MetaClass mc in classCollection
                          where mc.Namespace.CompareTo(@namespace) == 0
                          select new MetaClass(mc);

            return mciList.ToList();
        }

        // GET api/<controller>/5
        public MetaClass Get(int id)
        {
            Mediachase.MetaDataPlus.Configurator.MetaClass metaClass = Mediachase.MetaDataPlus.Configurator.MetaClass.Load(MetaDataContext.Instance, id);
            if (metaClass != null)
            {
                return new MetaClass(metaClass);
            }
            return null;
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }


        /// <summary>
        /// Removes a meta field from meta class
        /// </summary>
        /// <param name="metaClassId"></param>
        /// <param name="fieldId"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage RemoveField(int metaClassId, int fieldId)
        {
            MetaDataContext context = MetaDataContext.Instance;
            Mediachase.MetaDataPlus.Configurator.MetaClass metaClass = Mediachase.MetaDataPlus.Configurator.MetaClass.Load(context, metaClassId);
            if(metaClass == null)
            {
                HttpResponseMessage errorResponse = Request.CreateResponse<string>(HttpStatusCode.NotFound, "Meta Class with id " + metaClassId + " could not be loaded.", "application/json");
                return errorResponse;
            }

            MetaField field = MetaField.Load(MetaDataContext.Instance, fieldId);
            if (field == null)
            {
                HttpResponseMessage errorResponse = Request.CreateResponse<string>(HttpStatusCode.NotFound, "Meta Class with id " + metaClassId + " could not be loaded.", "application/json");
                return errorResponse;
            }
            
            
            metaClass.DeleteField(field);

            string message = JsonConvert.SerializeObject(new
            {
                Message = string.Format("Removed Meta Field '{0}' from Meta Class '{1}'", field.Name, metaClass.Name),
                MetaClassName = metaClass.Name,
                MetaFieldName = field.Name
            });
            
            HttpResponseMessage response = Request.CreateResponse<string>(HttpStatusCode.OK, "");
            response.Content = new StringContent(message);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return response;
        }


        // DELETE api/<controller>/5
        public HttpResponseMessage Delete(int id)
        {
            MetaDataContext context = MetaDataContext.Instance;
            Mediachase.MetaDataPlus.Configurator.MetaClass metaClass = Mediachase.MetaDataPlus.Configurator.MetaClass.Load(context, id);

            if (metaClass.IsSystem)
                throw new HttpException(500, "Cannot delete a system Meta Class");

            string message = JsonConvert.SerializeObject(new
            {
                Message = "Deleted Meta Class",
                Id = id,
                Name = metaClass.Name
            });

            Mediachase.MetaDataPlus.Configurator.MetaClass.Delete(MetaDataContext.Instance, id);

            HttpResponseMessage response = Request.CreateResponse<string>(HttpStatusCode.OK, "");
            response.Content = new StringContent(message);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return response;


        }
    }
}