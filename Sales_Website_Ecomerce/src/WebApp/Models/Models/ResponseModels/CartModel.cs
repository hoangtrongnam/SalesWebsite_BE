namespace Models.ResponseModels
{
    public class CartModel
    {
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public int QuantityMax { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
