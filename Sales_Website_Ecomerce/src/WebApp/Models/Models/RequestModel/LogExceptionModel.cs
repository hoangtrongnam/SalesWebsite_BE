namespace Models.RequestModel
{
    public class LogExceptionModel
    {
        public string UserName { get; set; }
        public string IPAddress { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string RequestPath { get; set; }
        public string RequestMethod { get; set; }
        public DateTime ExceptionDate { get; set; }
    }
}
