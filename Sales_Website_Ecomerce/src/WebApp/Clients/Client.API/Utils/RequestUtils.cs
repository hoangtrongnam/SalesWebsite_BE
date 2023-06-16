namespace Client.API.Utils
{
    public class RequestUtils
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public RequestUtils(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public int GetTenantId()
        {
            //var fromHeader = _contextAccessor.HttpContext?.Request.Headers["tenantId"];
            //if (!string.IsNullOrEmpty(fromHeader))
            //{
            //    return int.Parse(fromHeader ?? "1");
            //}
            //return -1;
            return 2;
        }

    }
}
