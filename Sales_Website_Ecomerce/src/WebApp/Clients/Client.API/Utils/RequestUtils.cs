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
            var fromHeader = _contextAccessor.HttpContext?.Request.Headers[TENANT_ID_HEADER];
            if (!string.IsNullOrEmpty(fromHeader))
            {
                return fromHeader ?? string.Empty;
            }
            return string.Empty;
        }

    }
}
