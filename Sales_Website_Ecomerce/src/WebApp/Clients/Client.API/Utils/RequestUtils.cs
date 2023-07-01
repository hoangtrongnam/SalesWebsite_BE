namespace Client.API.Utils
{
    public class RequestUtils
    {
        public const string TENANT_ID_HEADER = "TENANT_ID";
        private readonly IHttpContextAccessor _contextAccessor;

        public RequestUtils(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string GetTenantId()
        {
            //_contextAccessor.HttpContext?.Request.Headers.Add("tenantId", "DFD9DB98-448E-42F1-86FC-7AFA3D9C637A");
            var fromHeader = _contextAccessor.HttpContext?.Request.Headers["tenantId"];
            if (!string.IsNullOrEmpty(fromHeader))
            {
                return fromHeader;
            }
            else
            {
                throw new Exception("tenantId is required");
            }
            // return "DFD9DB98-448E-42F1-86FC-7AFA3D9C637A";
        }

    }
}
