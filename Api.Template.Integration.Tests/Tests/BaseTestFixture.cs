using System.Security.Claims;
using Api.Template.Integration.Tests.Builders;
using Api.Template.Integration.Tests.Config;
using Api.Template.Shared.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Api.Template.Integration.Tests.Tests
{
    public class BaseTestFixture : IDisposable
    {
        public TestServer ApiTestServer;

        public string? AccessToken { get; }

        public BaseTestFixture()
        {
            ApiTestServer = CreateTestServer();
            var config = ApiTestServer.Services.GetRequiredService<IConfiguration>();
            AccessToken = GetAccessToken(config);
        }
        public void Dispose()
        {
            ApiTestServer.Dispose();
        }

        private TestServer CreateTestServer()
        {
            //Environment.SetEnvironmentVariable("DatabaseConnectionString", "placeholder");


            var hostBuilder = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", true, true)
                        .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
                        .AddEnvironmentVariables();

                })
                .ConfigureLogging((context, logging) =>
                {
                    var config = context.Configuration.GetSection("Logging");
                    logging.AddConfiguration(config);
                    logging.AddConsole();
                })
                .ConfigureServices((_, services) =>
                {
                    services.AddScoped<IEnvironmentConfig, IntegrationEnvironmentConfig>();
                })
                .UseEnvironment("Integration")
                .UseStartup<TestStartup>();
            return new TestServer(hostBuilder);
        }

        private string GetAccessToken(IConfiguration config)
        {
            var authConfig = new TestsAuthConfig();
            config.GetSection("Auth").Bind(authConfig);
            var securityKey = !string.IsNullOrWhiteSpace(authConfig.JwtSymmetricKey)
                ? authConfig.JwtSymmetricKey
                : string.Empty;

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Convert.FromBase64String(securityKey)),
                SecurityAlgorithms.HmacSha256);

            var jwtDate = DateTime.Now;
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, "naveen.papisetty@outlook.com")
            };

            var token = new JwtTokenBuilder()
                .WithAuthConfig(authConfig)
                .WithNotBeforeDateTime(jwtDate)
                .WithExpiryDateTime(jwtDate.AddMinutes(60))
                .WithClaims(claims)
                .WithSigningCredentials(signingCredentials)
                .BuildToken();

            return token;
        }
    }
}
