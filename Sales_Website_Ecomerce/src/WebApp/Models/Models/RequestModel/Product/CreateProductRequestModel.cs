namespace Models.RequestModel.Product
{
    public class CreateOnlyProductRequestModel
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public int Status { get; set; }
        public List<ColorRequest> Colors { get; set; }
    }

    public class ColorRequest
    {
        public Guid ColorId { get; set; }
        public List<ImageRequest> Images { get; set; }
    }
    public class ImageRequest
    {
        public Guid ImageId { get; set; }
    }
}
