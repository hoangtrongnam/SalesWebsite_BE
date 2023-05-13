namespace Models.ResponseModels
{
    public class CartModel
    {
        public string? ProductID { get; set; } //sử dụng cho API Order
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public int QuantityMax { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class CartResponeModel
    {
        public int CartID { get; set; }
        public List<CartModel>? lstProduct{ get; set; }
    }
}
