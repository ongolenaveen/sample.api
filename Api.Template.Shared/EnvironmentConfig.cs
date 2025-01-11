using Api.Template.Shared.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Api.Template.Shared
{
    public class EnvironmentConfig(IConfiguration config) : IEnvironmentConfig
    {
        public string DatabaseConnectionString => Environment.GetEnvironmentVariable("DatabaseConnectionString") ?? string.Empty;
    }
}
