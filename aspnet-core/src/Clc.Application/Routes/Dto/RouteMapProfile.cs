using AutoMapper;

namespace Clc.Routes.Dto
{
    public class RouteMapProfile : Profile
    {
        public RouteMapProfile()
        {
            CreateMap<RouteTaskDto, RouteTask>()
                .ForMember(s => s.IdentifyTime, opt => opt.Ignore());
        }
    }
}
