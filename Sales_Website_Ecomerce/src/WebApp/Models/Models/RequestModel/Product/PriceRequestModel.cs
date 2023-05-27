namespace Models.RequestModel.Product
{
    public class PriceRequestModel
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public string Price { get; set; }
        public string PromotePrice { get; set; }
        public string PromotPercent { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string CreateBy { get; set; }
    }

    //public class ListPrice
    //{
    //    public List<PriceRequestModel> prices { get; set; }
    //    public string CreateBy { get; set; }
    //}
}
