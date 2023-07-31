namespace Models.ResponseModels.AttributeProduct
{
    public class ImageByColorResponseModel
    {
        public Guid ImageId { get; set; }
        public string ImageCode { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string SortOrder { get; set; }
    }
}
