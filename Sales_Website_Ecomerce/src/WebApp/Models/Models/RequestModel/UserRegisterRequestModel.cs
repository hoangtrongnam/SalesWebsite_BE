using System.ComponentModel.DataAnnotations;

namespace Models.RequestModel
{
    public class UserRegisterRequestModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set;}
        [Required]
        public string FullName { get; set;}
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set;}
        [Required]
        [Phone]
        public string PhoneNumber { get; set;}
        [Required]
        public string Address { get; set;}
    }
}
