namespace Models.RequestModel.ProductStock
{
    public class CreateProductStockRequestModel
    {
        public int ProductID { get; set; }
        public int SupplierID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int StatusID { get; set; }
        public int WareHouseID { get; set; }
        public string Description { get; set; }
        public string CreateBy { get; set; }
    }
}
