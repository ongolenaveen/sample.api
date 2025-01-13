namespace Api.Template.Shared.Interfaces
{
    public interface IEnvironmentConfig
    {
        public string DatabaseConnectionString { get;}

        public string CosmosAccountEndpoint { get; }

        public string CosmosAuthKey { get; }

        public string CosmosDatabaseName { get; }

        public string CosmosContainerName { get; }

    }
}