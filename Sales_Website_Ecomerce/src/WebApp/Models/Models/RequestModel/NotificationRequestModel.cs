using System.ComponentModel.DataAnnotations;

namespace Models.RequestModel
{
    public class NotificationRequestModel
    {
        [Required]
        public string? Content { get; set; }
        [Required]
        public string? Note { get; set; }
        [Required]
        public int? Status { get; set; }
        [Required]
        public Guid CreateBy { get; set; }
    }
}