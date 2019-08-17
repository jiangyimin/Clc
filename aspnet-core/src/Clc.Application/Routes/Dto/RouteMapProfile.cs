using AutoMapper;
using Clc.Authorization.Users;
using Clc.Routes;

namespace Clc.Routes.Dto
{
    public class RouteMapProfile : Profile
    {
        public RouteMapProfile()
        {
           CreateMap<RouteWorker, RouteWorkerDto>()
                .ForMember(x => x.ArticleList, opt => opt.Ignore());
        }
    }
}
