using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mediachase.Commerce;
using Mediachase.Commerce.Catalog.Dto;
using Mediachase.Commerce.Pricing;
using Newtonsoft.Json;

namespace EPiCode.Commerce.RestService.Converters
{
    public class CustomerPricingConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type valueType)
        {
            if (valueType == typeof(CustomerPricing))
                return true;

            return false;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteValue("null");
                return;
            }
            CustomerPricing pricing = (CustomerPricing)value;
            
            writer.WriteStartObject();

            writer.WritePropertyName("PriceTypeId");
            writer.WriteValue(pricing.PriceTypeId);

            writer.WritePropertyName("PriceCode");
            writer.WriteValue(pricing.PriceCode);

            writer.WriteEndObject();

        }
    }
}
