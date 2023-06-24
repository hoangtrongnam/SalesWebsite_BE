namespace Models.ResponseModels.Cart
{
    public class CartResponeModel
    {
        public Guid CartID { get; set; }
        public List<CartModel>? lstProduct { get; set; }
    }
}
