namespace Models.RequestModel.Product
{
    public class ImageRepositoryRequestModel
    {
        public Guid ImageID { get; set; }
        public string ImageCode { get; set; }
        public Guid ProductID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public int SortOrder { get; set; }
        public string CreateBy { get; set; }
    }
}
