namespace Models.ResponseModels.Product
{
    public class ProductResponseModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public int StatusID { get; set; }
        public int TenantID { get; set; }
        public string ImageName { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public string DescriptionImage { get; set; }
        public string SortOrder { get; set; }
        public string Price { get; set; }
        public string PromotePrice { get; set; }
        public string PromotPercent { get; set; }
        public string ExpirationDate { get; set; }
        public string EffectiveDate { get; set; }
        public string CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string UpdateDate { get; set; }
        public string UpdateBy { get; set; }
    }

    public class ImageResponseModel
    {
        public int ImageID { get; set; }
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string SortOrder { get; set; }
    }
    public class PriceResponseModel
    {
        public int PriceID { get; set; }
        public int ProductID { get; set; }
        public string Price { get; set; }
        public string PromotePrice { get; set; }
        public string PromotPercent { get; set; }
        public string ExpirationDate { get; set; }
        public string EffectiveDate { get; set; }
    }
}
