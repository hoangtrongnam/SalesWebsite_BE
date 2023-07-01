namespace Models.ResponseModels.Product
{
    public class ListProductResponseModel
    {
        public int TotalRecord { get; set; }
        public int TotalPage { get; set; }
        public List<ProductResponseModel> Products { get; set; }
    }

    public class ProductResponseModel
    {
        public Guid ProductID { get; set; }
        public string ProductCode { get; set; }
        public Guid CategoryID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public int Quantity { get; set; }
        public int Status { get; set; }
        public Guid TenantID { get; set; }
        public string CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public List<ImageResponseModel> Images { get; set; }
    }

    public class ImageResponseModel
    {
        public Guid ImageID { get; set; }
        public string ImageCode { get; set; }
        public Guid ProductID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string SortOrder { get; set; }
    }
    public class PriceResponseModel
    {
        public Guid PriceID { get; set; }
        public string PriceCode { get; set; }
        public Guid ProductID { get; set; }
        public string PromotePrice { get; set; }
        public string PromotPercent { get; set; }
        public string ExpirationDate { get; set; }
        public string EffectiveDate { get; set; }
        public int IsActive { get; set; }
    }
}
