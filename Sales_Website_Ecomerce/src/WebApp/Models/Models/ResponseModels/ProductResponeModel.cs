namespace Models.ResponseModels
{
    public class ProductResponeModel
    {
        public string? ProductId { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public int Quantity { get; set; }
        public string? Price { get; set; }
        public string? Description { get; set; }
        public string? CategoryName { get; set; }

        public Dictionary<int, string>? DictCategory { get; set; }
    }
}
