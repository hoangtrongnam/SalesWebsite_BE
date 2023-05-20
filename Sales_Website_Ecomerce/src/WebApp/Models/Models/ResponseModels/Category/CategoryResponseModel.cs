namespace Models.ResponseModels.Category
{
    public class CategoryResponseModel
    {
        public string ID { get; set; }
        public string Parent { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TenantID { get; set; }
        public string CreateBy { get; set; }
        public string CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public string UpdateDate { get; set; }
    }
}
