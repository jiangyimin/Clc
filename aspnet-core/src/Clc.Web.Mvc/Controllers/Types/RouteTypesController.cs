using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using Clc.Authorization;
using Clc.Types;
using Clc.Types.Dto;

namespace Clc.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Types)]
    public class RouteTypesController : ClcCrudController<RouteType, RouteTypeDto>
    {
        public RouteTypesController(IRepository<RouteType> repository)
            : base(repository)
        {
        }
	}
}