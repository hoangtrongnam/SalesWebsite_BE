namespace Models.ResponseModels.Cart
{
    public class CartResponeModel
    {
        public int CartID { get; set; }
        public List<CartModel>? lstProduct { get; set; }
    }
}
