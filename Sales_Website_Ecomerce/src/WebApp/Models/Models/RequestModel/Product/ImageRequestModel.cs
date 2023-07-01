namespace Models.RequestModel.Product
{
    public class ImageRequestModel
    {
        public Guid ProductID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
		public int SortOrder { get; set; }
    }
}
