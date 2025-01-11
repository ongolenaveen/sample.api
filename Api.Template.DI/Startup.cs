using System.Security.Claims;
using System.Text.Json.Serialization;
using Api.Template.Api;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace Api.Template.DI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration">Configuration</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configure Services.This method gets called by the runtime. 
        /// Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="services">Services</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddApiVersioning((options) =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });

            services.AddHttpContextAccessor();
            ConfigureCustomServices(services, Configuration);
            services.AddWebAppBindings(Configuration);
            services.AddSwaggerGen((options) => {
                var xmlFilename = $"{typeof(SampleController).Assembly.GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Sample API",
                    Version = "v1",
                    Description = @"The Sample API allows trusted consumers to view and maintain Customers.This API shall be accessed via a front-end application on behalf of customers. 
                                    Access to this API will be controlled via Azure AD (which implements OAuth2), and the consumer of this API is expected to get an OAuth (bearer) token before invoking the resources this API.",
                    Contact = new OpenApiContact
                    {
                        Name = "Sample API",
                        Email = "naveen.papisetty@outlook.com"
                    }
                });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"Authorization header using the Bearer scheme.
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer TJSQTgoG0wHmPMCbUbglBWUtJE2AVFiNvo303'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });
            var assembly = typeof(SampleController).Assembly;

            services.AddMvc((options) => { 
                    options.EnableEndpointRouting = false;
                }).AddApplicationPart(assembly)
                .AddJsonOptions(options => options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault)
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
                });

            services.AddHealthChecks();

            services.AddProblemDetails(options =>
            {
                options.CustomizeProblemDetails = (context) =>
                {
                    context.ProblemDetails.Type = context.ProblemDetails.Type;
                    context.ProblemDetails.Title = context.ProblemDetails.Title;
                    context.ProblemDetails.Detail = context.ProblemDetails.Detail;
                    context.ProblemDetails.Instance = context.HttpContext.Request.Path;
                    context.ProblemDetails.Extensions.Add("timestamp", DateTime.Now);
                };
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">App</param>
        /// <param name="env">Environment</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors((config) =>
            {
                config.AllowAnyOrigin();
                config.AllowAnyHeader();
                config.AllowAnyMethod();
            });

            app.UseSwagger(options => options.SerializeAsV2 = false);
            app.UseSwaggerUI(options => {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Sample API V1");
                options.OAuthClientId("Client Id");
                options.OAuthClientSecret("Client Secret Key");
                options.OAuthUseBasicAuthenticationWithAccessCodeGrant();
            });
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}");
                endpoints.MapHealthChecks("/health");
            });
        }

        protected virtual void ConfigureCustomServices(IServiceCollection services, IConfiguration configuration)
        {
            var azureAdInstance = Environment.GetEnvironmentVariable("AzureAdInstance");
            var azureAdTenantId = Environment.GetEnvironmentVariable("AzureAdTenantId");
            var azureAdClientId = Environment.GetEnvironmentVariable("AzureAdClientId");

            if (string.IsNullOrWhiteSpace(azureAdInstance))
                azureAdInstance = "https://login.microsoftonline.com/";

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(jwtBearerOptions =>
                {
                    jwtBearerOptions.IncludeErrorDetails = true;
                    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuer = true,
                        NameClaimType = ClaimTypes.Email
                    };
                }, identityOptions =>
                {
                    identityOptions.Instance = azureAdInstance;
                    identityOptions.TenantId = azureAdTenantId;
                    identityOptions.ClientId = azureAdClientId;
                });
        }
    }
}
