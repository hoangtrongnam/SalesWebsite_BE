namespace Models.RequestModel.Category
{
    public class CreateCategoryRepositoryRequestModel
    {
        public Guid CategoryID { get; set; }
        public string CategoryCode { get; set; }
        public int Value { get; set; }
        public int Parent { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
