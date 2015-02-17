using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mediachase.Commerce;
using Mediachase.Commerce.Catalog.Dto;
using Newtonsoft.Json;

namespace EPiCode.Commerce.RestService.Converters
{
    public class MoneyConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type valueType)
        {
            if (valueType == typeof(Money))
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
            Money money = (Money)value;
            
            writer.WriteStartObject();

            writer.WritePropertyName("Amount");
            writer.WriteValue(money.Amount);

            writer.WritePropertyName("Currency");
            writer.WriteValue(money.Currency.CurrencyCode);

            writer.WriteEndObject();

        }
    }
}
