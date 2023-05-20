namespace Models.RequestModel.Product
{
    public class CreateProductRequestModel
    {
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public int StatusID { get; set; }
        public int TenantID { get; set; }
        public string CreateBy { get; set; }
    }
}
