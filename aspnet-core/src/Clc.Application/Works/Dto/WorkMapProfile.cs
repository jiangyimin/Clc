using AutoMapper;
using Clc.Authorization.Users;
using Clc.Routes;

namespace Clc.Works.Dto
{
    public class WorkMapProfile : Profile
    {
        public WorkMapProfile()
        {
           CreateMap<Route, RouteCDto>()
                .ForMember(x => x.Workers, opt => opt.Ignore());
        }
    }
}
