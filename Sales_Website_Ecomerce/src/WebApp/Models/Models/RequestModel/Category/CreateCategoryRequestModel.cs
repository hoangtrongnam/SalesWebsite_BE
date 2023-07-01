namespace Models.RequestModel.Category
{
    public class CreateCategoryRequestModel
    {
        public int Value { get; set; }
        public int Parent { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
