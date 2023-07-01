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
    public class HoldProductRequestModel {
        public Guid OrderID { get; set; }
        public Guid ProductID { get; set; }
        public Guid WareHouseID { get; set; }
        public decimal ExfactoryPrice { get; set; }
        //public int StatusID { get; set; }
        public int HoldNumber { get; set; }
    }
}
