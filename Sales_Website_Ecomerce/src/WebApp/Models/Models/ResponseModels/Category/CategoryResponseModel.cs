namespace Models.ResponseModels.Category
{
    public class CategoryResponseModel
    {
        public Guid CategoryId { get; set; }
        public string CategoryCode { get; set; }
        public int Value { get; set; }
        public int Parent { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid TenantId { get; set; }
        public string CreateBy { get; set; }
        public string CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public string UpdateDate { get; set; }
    }
}
