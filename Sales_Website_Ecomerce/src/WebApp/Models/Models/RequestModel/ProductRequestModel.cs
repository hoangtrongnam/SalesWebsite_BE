using System.ComponentModel.DataAnnotations;

namespace Models.RequestModel
{
    public class ProductRequestModel
    {
        //public int ProductID { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? ImageProduct { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public string? Description { get; set; }
        public int StatusID { get; set; }
        public DateTime CreateBy { get; set; }
        public DateTime UpdateBy { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
