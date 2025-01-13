using Api.Template.Domain.Dto;

namespace Api.Template.Domain.Interfaces
{
    public interface ITagsDataProvider
    {
        Task<List<ItemTag>> GetTags(string itemNumber);

        Task UpsertTags(List<ItemTag> tags);
    }
}
