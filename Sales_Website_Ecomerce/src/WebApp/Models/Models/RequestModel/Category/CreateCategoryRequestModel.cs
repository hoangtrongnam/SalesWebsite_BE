namespace Models.RequestModel.Category
{
    public class CreateCategoryRequestModel
    {
        public int Parent { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TenantID { get; set; }
        public string CreateBy { get; set; }
    }
}
