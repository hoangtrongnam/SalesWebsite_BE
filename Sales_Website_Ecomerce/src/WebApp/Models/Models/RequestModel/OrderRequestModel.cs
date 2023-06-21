using System.ComponentModel.DataAnnotations;

namespace Models.RequestModel
{
    public class ProductModel
    {
        [Required]
        public int ProductID { get; set; } //sử dụng cho API Order
        [Required]
        public int Quantity { get; set; }
        //[Required]
        //public decimal Price { get; set; }
        [Required]
        public int WareHouseID { get; set; }
        [Required]
        public int PromoteID { get; set; }
    }
    public class OrderRequestModel
    {
        [Required]
        public int CartID { get; set; }
        [Required]
        public int CustomerID { get; set; }
        public decimal DepositAmount { get; set; }
        public string Note { get; set; }
        [Required]
        public int Status { get; set; }
        [Required]
        public List<ProductModel> lstProduct { get; set; }
    }
}