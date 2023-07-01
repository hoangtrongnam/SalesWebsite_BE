namespace Models.RequestModel.Product
{
    public class PriceRequestModel
    {
        public Guid ProductID { get; set; }
        public string PromotePrice { get; set; }
        public string PromotPercent { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime EffectiveDate { get; set; }
        public int IsActive { get; set; }
    }
}
