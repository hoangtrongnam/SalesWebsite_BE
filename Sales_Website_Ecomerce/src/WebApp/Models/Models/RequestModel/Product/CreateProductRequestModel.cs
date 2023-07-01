namespace Models.RequestModel.Product
{
    public class CreateOnlyProductRequestModel
    {
        public Guid CategoryID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public int Status { get; set; }
    }
}
