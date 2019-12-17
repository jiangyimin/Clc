using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using Clc.Authorization;
using Clc.Types;
using Clc.Types.Dto;

namespace Clc.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Types)]
    public class OilTypesController : ClcCrudController<OilType, OilTypeDto>
    {
        public OilTypesController(IRepository<OilType> repository)
            : base(repository)
        {
        }
	}
}