using System.ComponentModel.DataAnnotations;

namespace Models.RequestModel.Cart
{
    public class CartRequestModel
    {
        [Required]
        public int ProdutID { get; set; }
        [Required]
        public int CustomerID { get; set; }
        [Required]
        public int Quantity { get; set; }
        //[Required]
        //public int StatusID { get; set; }
        [Required]
        public int WarehouseID { get; set; }
    }
}
