namespace Client.API.Utils
{
    public class RequestUtils
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public RequestUtils(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string GetTenantId()
        {
            //var fromHeader = _contextAccessor.HttpContext?.Request.Headers["tenantId"];
            //if (!string.IsNullOrEmpty(fromHeader))
            //{
            //    return int.Parse(fromHeader ?? "1");
            //}
            //return -1;
            return "DFD9DB98-448E-42F1-86FC-7AFA3D9C637A";
        }

    }
}
