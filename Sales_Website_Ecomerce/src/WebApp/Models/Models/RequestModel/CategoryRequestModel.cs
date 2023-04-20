using System.ComponentModel.DataAnnotations;

namespace Models.RequestModel
{
    public class CategoryRequestModel
    {
       // public int CategoryID { get; set; }
        [Required]
        public string? Name { get; set; }
    }
}
