using System.ComponentModel.DataAnnotations;

namespace Models.RequestModel.Orders
{
    public class OrderCommonRequest
    {
        [Required]
        public Guid CartID { get; set; }
        [Required]
        public Guid CustomerID { get; set; }
        public decimal DepositAmount { get; set; }
        public string Note { get; set; }
        [Required]
        public int Status { get; set; }
    }
}
