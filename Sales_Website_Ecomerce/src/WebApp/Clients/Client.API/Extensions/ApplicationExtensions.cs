using Client.API.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Product.API.Extensions
{
    public static class ApplicationExtensions
    {
        public static void UseInfrastructure(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseRouting();
            //app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ExceptionLoggingMiddleware>();

            app.UseEndpoints(enpoints =>
            {
                enpoints.MapDefaultControllerRoute();
            });
        }
    }
}
