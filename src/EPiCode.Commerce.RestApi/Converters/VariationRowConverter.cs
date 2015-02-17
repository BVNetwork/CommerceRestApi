using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mediachase.Commerce.Catalog.Dto;
using Newtonsoft.Json;

namespace EPiCode.Commerce.RestService.Converters
{
    public class VariationRowConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type valueType)
        {
            if (valueType.FullName == "Mediachase.Commerce.Catalog.Dto.CatalogEntryDto+VariationRow")
                return true;

            return false;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
                writer.WriteValue("null");

            if (value is DBNull)
                writer.WriteValue("null");

            CatalogEntryDto.VariationRow row = value as CatalogEntryDto.VariationRow;
            if (row == null)
                writer.WriteValue("null");
            else
            {
                writer.WriteStartObject();

                writer.WritePropertyName("ListPrice");
                if (row.IsListPriceNull() == false)
                {
                    writer.WriteValue(row.ListPrice);
                }
                else
                {
                    writer.WriteValue("null");
                }
                writer.WriteEndObject();

            }
        }
    }
}
