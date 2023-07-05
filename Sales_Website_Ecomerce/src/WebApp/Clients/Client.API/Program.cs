using Client.API.Utils;
using Product.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<RequestUtils>();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "CORS",
                      policy =>
                      {
                          policy.WithOrigins("http://example.com",
                                              "http://www.contoso.com");
                      });
});

//builder.Services.AddFluentValidation();
//
var app = builder.Build();
// app.UseCors("CORS");
app.UseCors(x => x
           .AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader());
app.UseHttpsRedirection();
app.UseInfrastructure();
app.Run();
