namespace Models.ResponseModels.Cart
{
    public class CartModel
    {
        public string? Name { get; set; }
        public int Quantity { get; set; }
        public int QuantityMax { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public int WareHouseID { get; set; }
        public int QuantityMaxInWareHouse { get;}
    }
}
