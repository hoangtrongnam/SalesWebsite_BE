namespace Models.RequestModel.Category
{
    public class UpdateCategoryRequestModel
    {
        public int Parent { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UpdateBy { get; set; }
    }
}
