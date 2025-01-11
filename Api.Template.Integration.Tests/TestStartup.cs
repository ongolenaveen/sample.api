using System.Security.Claims;
using Api.Template.DI;
using Api.Template.Integration.Tests.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Api.Template.Integration.Tests
{
    public class TestStartup(IConfiguration configuration) : Startup(configuration)
    {
        protected override void ConfigureCustomServices(IServiceCollection services, IConfiguration configuration)
        {
            var authConfig = new TestsAuthConfig();
            configuration.GetSection("Auth").Bind(authConfig);
            var securityKey = !string.IsNullOrWhiteSpace(authConfig.JwtSymmetricKey)
                ? authConfig.JwtSymmetricKey
                : string.Empty;

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    var symmetricKey = new SymmetricSecurityKey(Convert.FromBase64String(securityKey));
                    options.IncludeErrorDetails = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = symmetricKey,
                        ValidAudience = authConfig.Audience,
                        ValidIssuer = authConfig.Issuer,
                        RequireSignedTokens = true,
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        NameClaimType = ClaimTypes.NameIdentifier
                    };
                });
        }
    }
}
