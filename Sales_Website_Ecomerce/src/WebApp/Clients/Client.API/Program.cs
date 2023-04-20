using Product.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddInfrastructure(builder.Configuration);
var app = builder.Build();

app.UseInfrastructure();

app.Run();
