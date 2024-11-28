using AutoMapper;
using NZWalks.API.Models.Domian;
using NZWalks.API.Models.DTO;
using NZWalks.API.Models.DTO.Difficulty;
using NZWalks.API.Models.DTO.Walks;

namespace NZWalks.API.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Region, RegionDto>().ReverseMap();
            CreateMap<AddRegionRequestDto, Region>().ReverseMap();
            CreateMap<UpdateRegionRequestDto, Region>().ReverseMap();
            CreateMap<AddWalksRequestDto,Walk>().ReverseMap();
            CreateMap<Walk,WalksDto>().ReverseMap();
            CreateMap<Difficulty,DifficultyDto>().ReverseMap();
            CreateMap<UpdateWalkRequestDto,Walk>().ReverseMap();
        }
    }

    //    /* public AutoMapperProfile()
    //     {
    //        CreateMap<SourcClass, DestinationClass>().ForMember(x => x.DestinationName, opt => opt.MapFrom(y => y.SourceName)).ReverseMap();
    //     } 


    //public class SourcClass
    //{
    //    public string SourceName { get; set; } = "This is a destination map object";

    //}
    //public class DestinationClass
    //{
    //    public string DestinationName { get; set; } =string.Empty;

    //}
}
