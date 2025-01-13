namespace Api.Template.Cosmos.DataProvider.Documents
{
    public interface ICosmosDocument
    {
        string Id { get; set; }

        string PKey { get; set; }
    }
}
