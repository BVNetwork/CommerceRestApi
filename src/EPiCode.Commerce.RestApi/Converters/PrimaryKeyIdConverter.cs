using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mediachase.BusinessFoundation.Data;
using Newtonsoft.Json;

namespace EPiCode.Commerce.RestService.Converters
{
    /// <summary>
    /// The default JSON Serialization of the PrimaryKeyId fails
    /// </summary>
    public class PrimaryKeyIdConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(PrimaryKeyId);
        }
        public override bool CanRead
        {
            get
            {
                return false;
            }
        }
        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var primaryKey = (PrimaryKeyId)value;
            // Just write the guid raw
            writer.WriteValue(primaryKey.ToString());
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // the default deserialization works fine, 
            // but otherwise we'd handle that here
            throw new NotImplementedException();
        }
    }
}
