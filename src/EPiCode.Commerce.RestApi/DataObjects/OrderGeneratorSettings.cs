namespace ServiceApi.DataObjects
{
    public class OrderGeneratorSettings
    {
        public int NumberOfOrdersToGenerate { get; set; }
        public int MaximumLineItemsPerOrder { get; set; }

    }
}