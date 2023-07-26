using System.ComponentModel.DataAnnotations;

namespace Models.RequestModel.Orders
{
    public class OrderCommonRequest
    {
        [Required]
        public Guid CartId { get; set; }
        [Required]
        public Guid CustomerId { get; set; }
        public decimal DepositAmount { get; set; }
        public string Note { get; set; }
        [Required]
        public int Status { get; set; }
    }
}
