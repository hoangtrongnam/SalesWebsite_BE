using System.ComponentModel.DataAnnotations;

namespace Models.RequestModel.Cart
{
    public class CartRequestModel
    {
        [Required]
        public Guid ProductId { get; set; }
        [Required]
        public Guid CustomerId { get; set; }
        [Required]
        public int Quantity { get; set; }
        //[Required]
        //public int StatusID { get; set; }
        [Required]
        public Guid WarehouseId { get; set; }
        [Required]
        public Guid PromoteId { get; set; }
    }
}
