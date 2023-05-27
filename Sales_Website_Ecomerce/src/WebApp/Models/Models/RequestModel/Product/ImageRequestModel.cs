namespace Models.RequestModel.Product
{
    public class ImageRequestModel
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
		public int SortOrder { get; set; }
        public string CreateBy { get; set; }
    }
    //public class ListImage
    //{
    //    public List<ImageRequestModel> Images { get; set; }
    //    public string CreateBy { get; set; }
    //}
}
