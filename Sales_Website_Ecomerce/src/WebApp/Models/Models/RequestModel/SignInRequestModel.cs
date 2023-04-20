using System.ComponentModel.DataAnnotations;

namespace Models.RequestModel
{
    public class SignInRequestModel
    {
        [Required,EmailAddress]
        public string EmaiAddress { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
