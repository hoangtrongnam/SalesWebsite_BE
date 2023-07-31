namespace Models.RequestModel.ProductStock
{
    public class CreateProductStockRequestModel
    {
        public Guid ProductID { get; set; }
        public Guid SupplierID { get; set; }
        public Guid ColorID { get; set; }
        public Guid SizeID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Status { get; set; }
        public string ImportPrice { get; set; }
        public string ExfactoryPrice { get; set; }
        public Guid WareHouseID { get; set; }
        public string Description { get; set; }
    }
    public class HoldProductRequestModel {
        public Guid OrderID { get; set; }
        public List<LstHoldProduct> LstHoldProducts { get; set; }
    }
    public class LstHoldProduct {
        public Guid ProductID { get; set; }
        public Guid WareHouseID { get; set; }
        public decimal ExfactoryPrice { get; set; }
        //public int StatusID { get; set; }
        public int HoldNumber { get; set; }
    }
}
