using System.ComponentModel.DataAnnotations;

namespace Models.RequestModel
{
    public class JobRequestModel
    {
        [Required]
        public string JobName { get; set; }
        [Required]
        public int Status { get; set; }
        public string Content { get; set; }
    }
}