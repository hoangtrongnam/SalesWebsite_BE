using Client.API.Exceptions;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Models.RequestModel.Product;
using Services;
using Services.Automapper;
using System.Reflection;
using System.Text;
using UnitOfWork.Database;
using UnitOfWork.Interface;
using Validator.Product;

namespace Product.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(FluentValidationActionFilter));
            }).ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddApiVersioning(opt =>
            {
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.ReportApiVersions = true;
                opt.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                                                                new HeaderApiVersionReader("x-api-version"),
                                                                new MediaTypeApiVersionReader("x-api-version"));
            });

            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Sales Website API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                option.IncludeXmlComments(xmlPath);
                
            });


            services.AddInfrastructureServices();

            services.AddFluentValidation();
            services.AddTransient<IValidator<CreateOnlyProductRequestModel>, CreateProductRequestModelValidator>();

            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = configuration["JWT:ValidAudience"],
                    ValidIssuer = configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
                };
            });
            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 1024 * 1024 * 10; // Giới hạn 10MB
            });
            services.AddAutoMapper(typeof(MappingProfile));
            return services;
        }

        private static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWorkDatabase>();
            services.AddTransient<IProductServices, ProductServices>();
            services.AddTransient<IAccountServices, AccountServices>();
            services.AddTransient<ICategoryServices, CategoryServices>();
            services.AddTransient<ICartServices, CartServices>();
            services.AddTransient<IWareHouseServices, WareHouseServices>();
            services.AddTransient<ISupplierServices, SupplierServices>();
            services.AddTransient<IProductStockService, ProductStockService>();
            services.AddTransient<ICommonService, CommonService>();
            services.AddTransient<OrderServices>();
            services.AddTransient<IAttributeProductService, AttributeProductService>();
            return services;
        }
    }
}
