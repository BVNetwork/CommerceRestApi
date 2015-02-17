using Mediachase.MetaDataPlus.Configurator;

namespace EPiCode.Commerce.RestService.DataObjects
{
    public class MetaField
    {
        public MetaField()
        {

        }

        public MetaField(Mediachase.MetaDataPlus.Configurator.MetaField metaField)
        {
            Id = metaField.Id;
            IsSystem = metaField.IsSystem;
            Name = metaField.Name;
            Description = metaField.Description;
            FriendlyName = metaField.FriendlyName;
            IsUser = metaField.IsUser;
            Namespace = metaField.Namespace;
            MultiLanguageValue = metaField.MultiLanguageValue;
            Tag = metaField.Tag;
            Length = metaField.Length;
            OwnerMetaClassIdList = metaField.OwnerMetaClassIdList;
            SafeAllowSearch = metaField.SafeAllowSearch;
            AllowNulls = metaField.AllowNulls;
            DataType = metaField.DataType;
            DataTypeName = metaField.DataType.ToString();
        }

        public bool AllowNulls { get; set; }
        public MetaDataType DataType { get; set; }
        public string DataTypeName { get; set; }
        public bool MultiLanguageValue { get; set; }
        public int Length { get; set; }
        public bool SafeAllowSearch { get; set; }
        public MetaClassIdCollection OwnerMetaClassIdList { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsSystem { get; set; }
        public string Description { get; set; }
        public string FriendlyName { get; set; }
        public bool IsUser { get; set; }
        public string Namespace { get; set; }
        public string TableName { get; set; }
        public object Tag { get; set; }

    }
}