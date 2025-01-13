using Api.Template.Cosmos.DataProvider.AutoMapperProfiles;
using Api.Template.Cosmos.DataProvider.DataProviders;
using Api.Template.Domain.Interfaces;
using Api.Template.Shared.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Template.Cosmos.DataProvider
{
    public class CosmosDataProviderStartup : IDataProviderStartup
    {
        public void AddDataProviders(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITagsDataProvider, TagsDataProvider>();
            services.AddAutoMapper(typeof(TagsProfile));

            var serviceProvider = services.BuildServiceProvider();
            var databaseConfig = serviceProvider.GetRequiredService<IEnvironmentConfig>();

            var cosmosEndpoint = databaseConfig.CosmosAccountEndpoint;
            var cosmosAuthKey = databaseConfig.CosmosAuthKey;
            var databaseName = databaseConfig.CosmosDatabaseName;
            var containerName = databaseConfig.CosmosContainerName;
            var cosmosClientOptions = new CosmosClientOptions
            {
                SerializerOptions = new CosmosSerializationOptions
                {
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase,
                }
            };
            services.AddSingleton((_) =>
            {
                var cosmosClient = new CosmosClient(cosmosEndpoint, cosmosAuthKey, cosmosClientOptions);

                var databaseResponse = cosmosClient.CreateDatabaseIfNotExistsAsync(
                    id: databaseName,
                    throughput: 4000
                ).Result;

                var database = databaseResponse.Database;

                var containerResponse = database.CreateContainerIfNotExistsAsync(
                    id: containerName,
                    partitionKeyPath: "/pKey"
                ).Result;

                var container = containerResponse.Container;
                return container;
            });
        }
    }
}
