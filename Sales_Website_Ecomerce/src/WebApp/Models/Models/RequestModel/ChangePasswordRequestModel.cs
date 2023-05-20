using System.ComponentModel.DataAnnotations;

namespace Models.RequestModel
{
    public class ChangePasswordRequestModel
    {
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public string PasswordOld { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
