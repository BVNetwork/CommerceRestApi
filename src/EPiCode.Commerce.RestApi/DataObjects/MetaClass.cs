using Mediachase.MetaDataPlus.Configurator;

namespace EPiCode.Commerce.RestService.DataObjects
{
    public class MetaClass
    {
        public MetaClass()
        {
        }

        public MetaClass(Mediachase.MetaDataPlus.Configurator.MetaClass metaClass)
        {

            Id = metaClass.Id;
            IsSystem = metaClass.IsSystem;
            Name = metaClass.Name;
            Description = metaClass.Description;
            FriendlyName = metaClass.FriendlyName;
            IsAbstract = metaClass.IsAbstract;
            IsUser = metaClass.IsUser;
            MetaClassType = metaClass.MetaClassType;
            MetaClassTypeName = metaClass.MetaClassType.ToString();
            Namespace = metaClass.Namespace;
            TableName = metaClass.TableName;
            Tag = metaClass.Tag;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsSystem { get; set; }
        public string Description { get; set; }
        public string FriendlyName { get; set; }
        public bool IsAbstract { get; set; }
        public bool IsUser { get; set; }
        public MetaClassType MetaClassType { get; set; }
        public string MetaClassTypeName { get; set; }
        public string Namespace { get; set; }
        public string TableName { get; set; }
        public object Tag { get; set; }
    }
}