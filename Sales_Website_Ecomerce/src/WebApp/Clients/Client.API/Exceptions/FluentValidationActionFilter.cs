using Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Client.API.Exceptions
{
    public class FluentValidationActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {

            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values
                           .SelectMany(v => v.Errors)
                           .Select(e => e.ErrorMessage)
                           .ToList();

                var response = ApiResponse<object>.ErrorResponse("Validation Error");
                response.Message = string.Join("\n", errors);

                var result = new ObjectResult(response)
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };

                context.Result = result;
            }

            base.OnActionExecuting(context);
        }
    }
}
