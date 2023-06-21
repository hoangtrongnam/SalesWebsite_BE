namespace Models.RequestModel.Product
{
    public class CreateOnlyProductRepositoryRequestModel
    {
        public Guid ProductID { get; set; }
        public string ProductCode { get; set; }
        public Guid CategoryID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public int Status { get; set; }
        public string CreateBy { get; set; }
    }
}
