namespace Models.ResponseModels
{
    public class UserResponseModel
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string PasswordHasd { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string CreateDate { get; set; }
        public string UpdateDate { get; set; }
        public string TenanID { get; set; }
    }
}
