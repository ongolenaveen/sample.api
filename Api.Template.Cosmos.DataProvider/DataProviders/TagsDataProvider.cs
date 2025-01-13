using Api.Template.Cosmos.DataProvider.Documents;
using Api.Template.Domain.Dto;
using Api.Template.Domain.Interfaces;
using AutoMapper;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace Api.Template.Cosmos.DataProvider.DataProviders
{
    public class TagsDataProvider(
        Container container,
        IMapper mapper,
        ILogger<TagsDataProvider> logger)
        : ITagsDataProvider
    {
        private readonly Container _container = container ?? throw new ArgumentNullException(nameof(container));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly ILogger<TagsDataProvider> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        /// <summary>
        /// Gets tags for a given item number.
        /// </summary>
        /// <param name="itemNumber">item number for which tags needs to be retrieved.</param>
        /// <returns>Tags.</returns>
        /// 
        public async Task<List<ItemTag>> GetTags(string itemNumber)
        {
            var itemTagDocuments = new List<ItemTagDocument>();
            var whereClause = "";
            var selectClause = "SELECT * FROM objects o";
            if (!string.IsNullOrWhiteSpace(itemNumber))
                whereClause += "o.itemNumber =@itemNumber";

            var queryString = $"{selectClause} WHERE {whereClause}";

            var query = new QueryDefinition(query: queryString)
                .WithParameter("@itemNumber", itemNumber);

            using var feed = _container.GetItemQueryIterator<ItemTagDocument>(
                queryDefinition: query
            );

            while (feed.HasMoreResults)
            {
                var response = await feed.ReadNextAsync();
                itemTagDocuments.AddRange(response.Select(item => item));
            }

            var objectTags = _mapper.Map<IEnumerable<ItemTagDocument>, IEnumerable<ItemTag>>(itemTagDocuments);

            return objectTags.ToList();
        }

        /// <summary>
        /// Insert/Update tags for a given ItemNumber.
        /// </summary>
        /// <param name="tags">Tags which needs to be inserted/updated. </param>
        /// <returns>None.</returns>
        public async Task UpsertTags(List<ItemTag> tags)
        {
            _logger.LogInformation("Inserting/Updating into cosmos data base.");

            var objectTagDocuments = _mapper.Map<IEnumerable<ItemTag>, IEnumerable<ItemTagDocument>>(tags);
            foreach (var document in objectTagDocuments)
                await _container.UpsertItemAsync(document);
        }
    }
}
