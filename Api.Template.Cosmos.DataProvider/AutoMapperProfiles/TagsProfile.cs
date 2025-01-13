using Api.Template.Cosmos.DataProvider.Documents;
using Api.Template.Domain.Dto;
using AutoMapper;

namespace Api.Template.Cosmos.DataProvider.AutoMapperProfiles
{
    public class TagsProfile : Profile
    {
        public TagsProfile()
        {
            CreateMap<ItemTag, ItemTagDocument>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ItemId))
                .ForMember(dest => dest.PKey, opt => opt.MapFrom(src => "tags"))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags))
                .ReverseMap();
        }
    }
}
