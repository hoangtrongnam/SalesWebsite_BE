namespace Models.ResponseModels.Cart
{
    public class CartResponeModel
    {
        public Guid CartId { get; set; }
        public decimal TotalPayment { get; set; } //tổng số tiền cần thanh toán
        public List<CartModel>? lstProduct { get; set; }
    }
}
