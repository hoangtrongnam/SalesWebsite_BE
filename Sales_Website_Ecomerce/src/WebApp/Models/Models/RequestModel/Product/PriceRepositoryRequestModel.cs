namespace Models.RequestModel.Product
{
    public class PriceRepositoryRequestModel
    {
        public Guid PriceID { get; set; }
        public string PriceCode { get; set; }
        public Guid ProductID { get; set; }
        public string PromotePrice { get; set; }
        public string PromotPercent { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string CreateBy { get; set; }
        public int IsActive { get; set; }
    }
}
