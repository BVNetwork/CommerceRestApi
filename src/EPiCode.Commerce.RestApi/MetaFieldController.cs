using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using log4net;
using Mediachase.MetaDataPlus;
using Mediachase.MetaDataPlus.Configurator;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace EPiCode.Commerce.RestService
{
    public class MetaFieldController : SecuredApiController
    {
        // GET api/<controller>
        public IEnumerable<DataObjects.MetaField> Get()
        {
            MetaFieldCollection fieldCollection = Mediachase.MetaDataPlus.Configurator.MetaField.GetList(MetaDataContext.Instance);
            var mciList = from Mediachase.MetaDataPlus.Configurator.MetaField mc in fieldCollection
                          select new DataObjects.MetaField(mc);

            return mciList.ToList();  
        }

        public IEnumerable<DataObjects.MetaField> GetAdvanced(string @namespace, bool isSystem)
        {
            MetaFieldCollection fieldCollection = Mediachase.MetaDataPlus.Configurator.MetaField.GetList(MetaDataContext.Instance);
            IEnumerable<DataObjects.MetaField> mciList = from Mediachase.MetaDataPlus.Configurator.MetaField mc in fieldCollection
                          where mc.Namespace.CompareTo(@namespace) == 0 && mc.IsSystem == isSystem
                          select new DataObjects.MetaField(mc);
            return mciList.ToList();
        }

        // GET api/<controller>
        public IEnumerable<DataObjects.MetaField> GetNonSystemFields()
        {
            MetaFieldCollection fieldCollection = Mediachase.MetaDataPlus.Configurator.MetaField.GetList(MetaDataContext.Instance);
            var mciList = from Mediachase.MetaDataPlus.Configurator.MetaField mc in fieldCollection
                          where mc.IsSystem == false
                          select new DataObjects.MetaField(mc);

            return mciList.ToList();
        }

        // GET api/<controller>
        public IEnumerable<DataObjects.MetaField> GetUnusedFields()
        {
            MetaFieldCollection fieldCollection = Mediachase.MetaDataPlus.Configurator.MetaField.GetList(MetaDataContext.Instance);
            var mciList = from Mediachase.MetaDataPlus.Configurator.MetaField mc in fieldCollection
                          where mc.OwnerMetaClassIdList.Count == 0
                          select new DataObjects.MetaField(mc);

            return mciList.ToList();
        }

        [HttpDelete]
        public IEnumerable<JObject> DeleteUnusedFields()
        {
            MetaFieldCollection fieldCollection = Mediachase.MetaDataPlus.Configurator.MetaField.GetList(MetaDataContext.Instance);
            var mciList = from Mediachase.MetaDataPlus.Configurator.MetaField mc in fieldCollection
                          where mc.OwnerMetaClassIdList.Count == 0
                          select new DataObjects.MetaField(mc);


            MetaDataContext context = MetaDataContext.Instance;
            
            List<JObject> result = new List<JObject>();

            foreach (DataObjects.MetaField field in mciList)
            {
                MetaField metaField = MetaField.Load(context, field.Id);
                if (metaField.IsSystem)
                {
                    result.Add(JObject.FromObject(
                    new 
                    {
                        Result = "Failed",
                        Id = metaField.Id,
                        Name = metaField.Name,
                        Message = string.Format("Cannot delete system field {0} ({1})", metaField.Name, metaField.Id)
                    }));
                }
                else
                {
                    bool isInUse = false;
                    List<string> usedBy = new List<string>();
                    foreach (object ownerId in metaField.OwnerMetaClassIdList)
                    {
                        isInUse = true;
                        MetaClass ownerClass = MetaClass.Load(context, int.Parse(ownerId.ToString()));
                        usedBy.Add(string.Format("{0} ({1})", ownerClass.Name, ownerId.ToString()));
                    }

                    if (isInUse == false)
                    {
                        MetaField.Delete(context, metaField.Id);
                        result.Add(JObject.FromObject(
                        new
                        {
                            Result = "OK",
                            Message = "Deleted Meta Field",
                            Name = metaField.Name,
                            Id = metaField.Id
                        }));
                        
                    }
                    else
                    {
                        result.Add(JObject.FromObject(
                        new 
                        {
                            Result = "Failed",
                            Message = string.Format("Meta Field with id {0} is in use. Cannot delete.", metaField.Id),
                            Name = metaField.Name,
                            Id = metaField.Id,
                            UsedBy = usedBy
                        }));
                    }
                }

            }

            return result;
        }


        // GET api/<controller>/5
        public DataObjects.MetaField Get(int id)
        {
            MetaField field = MetaField.Load(MetaDataContext.Instance, id);
            if (field != null)
            {
                var mf = new DataObjects.MetaField(field);
                return mf;
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

        // DELETE api/<controller>/5
        public HttpResponseMessage Delete(int id)
        {
            MetaDataContext context = MetaDataContext.Instance;
            MetaField metaField = MetaField.Load(context, id);
            if(metaField.IsSystem)
                throw new HttpException(500, "Cannot delete a system field");

            bool isInUse = false;
            List<string> usedBy = new List<string>();
            foreach (object ownerId in metaField.OwnerMetaClassIdList)
            {
                isInUse = true;
                MetaClass ownerClass = MetaClass.Load(context, int.Parse(ownerId.ToString()));
                usedBy.Add(string.Format("{0} ({1})", ownerClass.Name, ownerId.ToString()));
            }

            if(isInUse == false)
            {
                MetaField.Delete(context, id);
                string message = JsonConvert.SerializeObject(new
                {
                    Message = "Deleted Meta Field",
                    Id = id
                });
                return CreateResponseMessage(message);
            }
            else
            {
                string message = JsonConvert.SerializeObject(new
                {
                    Message = "Meta Field with id " + id + " is in use. Cannot delete.",
                    UsedBy = usedBy
                });

                return CreateResponseMessage(HttpStatusCode.Forbidden, message);
            }
        }

    }
}