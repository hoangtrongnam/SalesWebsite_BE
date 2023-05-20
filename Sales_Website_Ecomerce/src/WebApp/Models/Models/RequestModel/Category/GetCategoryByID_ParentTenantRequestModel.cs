namespace Models.RequestModel.Category
{
    public class GetCategoryByID_ParentTenantRequestModel
    {
        public int ID { get; set; }
        public int Parent { get; set; }
        public int TenantID { get; set; }
    }
}
