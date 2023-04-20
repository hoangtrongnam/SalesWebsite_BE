using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Client.API.Exceptions
{
    public class CustomExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            ExceptionManager.HandleException(context.Exception);
            context.Result = new StatusCodeResult(500);
        }
    }
}
