namespace Models.RequestModel.Category
{
    public class UpdateCategoryRequestModel
    {
        public string Parent { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UpdateBy { get; set; }
    }
}
