namespace Models.ResponseModels.Cart
{
    public class CartResponeModel
    {
        public int CartID { get; set; }
        public decimal TotalPayment { get; set; } //tổng số tiền cần thanh toán
        public List<CartModel>? lstProduct { get; set; }
    }
}
