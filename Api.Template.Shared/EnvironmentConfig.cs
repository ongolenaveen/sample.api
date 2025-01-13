using Api.Template.Shared.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Api.Template.Shared
{
    public class EnvironmentConfig(IConfiguration config) : IEnvironmentConfig
    {
        public string DatabaseConnectionString => Environment.GetEnvironmentVariable("DatabaseConnectionString") ?? string.Empty;

        public string CosmosAccountEndpoint => Environment.GetEnvironmentVariable("CosmosAccountEndpoint") ?? string.Empty;

        public string CosmosAuthKey => Environment.GetEnvironmentVariable("CosmosAuthKey") ?? string.Empty;

        public string CosmosDatabaseName => Environment.GetEnvironmentVariable("CosmosDatabaseName") ?? string.Empty;

        public string CosmosContainerName => Environment.GetEnvironmentVariable("CosmosContainerName") ?? string.Empty;
    }
}
