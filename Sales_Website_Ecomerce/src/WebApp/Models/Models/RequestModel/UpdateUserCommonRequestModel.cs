namespace Models.RequestModel
{
    public class UpdateUserCommonRequestModel
    {      
        public string UserName { get; set; }    
        public string Password { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }
}
