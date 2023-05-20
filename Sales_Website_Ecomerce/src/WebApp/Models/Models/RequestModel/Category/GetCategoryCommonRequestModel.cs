namespace Models.RequestModel.Category
{
    public class GetCategoryCommonRequestModel
    {
        public int ID { get; set; }
        public int TenantID { get; set; }
        public int Parent { get; set; }
        public string Name { get; set; }
    }
}
