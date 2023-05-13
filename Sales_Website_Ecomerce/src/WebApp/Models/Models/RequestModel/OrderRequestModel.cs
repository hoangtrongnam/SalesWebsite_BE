using System.ComponentModel.DataAnnotations;

namespace Models.RequestModel
{
    public class ProductModel
    {
        [Required]
        public int ProductID { get; set; } //sử dụng cho API Order
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal Price { get; set; }
        //public decimal TotalPrice { get; set; }
    }
    public class OrderRequestModel
    {
        [Required]
        public int CartID { get; set; }
        [Required]
        public int CustomerID { get; set; }
        //public decimal TotalPayment { get; set; }
        [Required]
        public List<ProductModel> lstProduct { get; set; }
    }
}
