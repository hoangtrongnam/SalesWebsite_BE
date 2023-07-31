namespace Models.RequestModel.ProductStock
{
    public class CreateProductStockRepositoryRequestModel
    {
        public Guid ProductStockID { get; set; }
        public string ProductStockCode { get; set; }
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
}
