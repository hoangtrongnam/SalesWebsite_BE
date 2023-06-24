using Models.RequestModel;
using Services;

namespace Client.API.Exceptions
{
    public class ExceptionLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionLoggingMiddleware> _logger;
        private readonly ICommonService _commonService;

        public ExceptionLoggingMiddleware(RequestDelegate next, ILogger<ExceptionLoggingMiddleware> logger, ICommonService commonService)
        {
            _next = next;
            _logger = logger;
            _commonService = commonService;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Ghi log exception xuống cơ sở dữ liệu
                LogExceptionToDatabase(ex, context);

                //Write Log exception file
                //string LogDirectory = _commonService.GetConfigValueService((int)Common.Enum.ConfigKey.PathLogException);
                //ExceptionManager.HandleException(ex, LogDirectory);

                // Set StatusCode trả về cho client
                context.Response.StatusCode = 500;
            }
        }

        private void LogExceptionToDatabase(Exception exception, HttpContext context)
        {
            try
            {
                // Lấy thông tin địa chỉ IP từ HttpContext.Connection
                var ipAddress = context.Connection.RemoteIpAddress?.ToString();
              
                // Lấy thông tin người dùng từ HttpContext.User
                var user = "";//context.User.Identity?.Name;

                var logModel = new LogExceptionModel()
                {
                    UserName = user,
                    IPAddress = ipAddress,
                    Message = exception.Message,
                    StackTrace = exception.StackTrace,
                    RequestPath = context.Request.Path,
                    RequestMethod = context.Request.Method,
                    ExceptionDate = DateTime.UtcNow
                };

                //Write log database
                _commonService.LogExceptionToDatabase(logModel);
            }
            catch (Exception ex)
            {
                //WriteLog File
                string LogDirectory = _commonService.GetConfigValueService((int)Common.Enum.ConfigKey.PathLogException);
                ExceptionManager.HandleException(ex, LogDirectory);
            }
        }
    }
}
