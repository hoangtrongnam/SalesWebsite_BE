namespace Models.ResponseModels.ProductStocks
{
    public class ProductStockResponseModel
    {
        public Guid OrderID { get; set; }
        public Guid ProductID { get; set; }
        public Guid WareHouseID { get; set; }
        public decimal? ExfactoryPrice { get; set; }
        //public int StatusID { get; set; }
        public int? HoldNumber { get; set; }
        public List<string> Code { get;}
    }
}
