namespace Models.RequestModel.Product
{
    public class CreateOnlyProductRequestModel
    {
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public int StatusID { get; set; }
        public int TenantID { get; set; }
        public string CreateBy { get; set; }
    }

    //public class CreateProductRequestModel
    //{
    //    public CreateOnlyProductRequestModel product { get; set; }
    //    public List<ImageRequestModel> images { get; set; }
    //    public List<PriceRequestModel> prices { get; set; }
    //}
}
