using System.ComponentModel.DataAnnotations;

namespace Models.RequestModel.Cart
{
    public class CartRequestModel
    {
        [Required]
        public Guid ProdutID { get; set; }
        [Required]
        public Guid CustomerID { get; set; }
        [Required]
        public int Quantity { get; set; }
        //[Required]
        //public int StatusID { get; set; }
        [Required]
        public Guid WarehouseID { get; set; }
        [Required]
        public Guid PromoteID { get; set; }
    }
}
