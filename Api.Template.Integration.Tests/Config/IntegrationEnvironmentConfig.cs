using Api.Template.Shared.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Api.Template.Integration.Tests.Config
{
    public class IntegrationEnvironmentConfig(IConfiguration config) : IEnvironmentConfig
    {
        public string DatabaseConnectionString => Environment.GetEnvironmentVariable("DatabaseConnectionString") ?? string.Empty;
    }
}
