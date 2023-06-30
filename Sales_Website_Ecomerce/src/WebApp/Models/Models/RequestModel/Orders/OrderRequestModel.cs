using System.ComponentModel.DataAnnotations;

namespace Models.RequestModel.Orders
{
    public class ProductModel
    {
        [Required]
        public Guid ProductID { get; set; } //sử dụng cho API Order
        [Required]
        public int Quantity { get; set; }
        //[Required]
        //public decimal Price { get; set; }
        [Required]
        public Guid WareHouseID { get; set; }
        [Required]
        public Guid PromoteID { get; set; }
        //[Required]
        //public Guid CartProductID { get; set; }
    }
    public class OrderRequestModel : OrderCommonRequest
    {
        [Required]
        public List<ProductModel> lstProduct { get; set; }
    }
}