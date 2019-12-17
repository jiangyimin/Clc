using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using Clc.Authorization;
using Clc.Types;
using Clc.Types.Dto;

namespace Clc.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Types)]
    public class VehicleMTTypesController : ClcCrudController<VehicleMTType, VehicleMTTypeDto>
    {
        public VehicleMTTypesController(IRepository<VehicleMTType> repository)
            : base(repository)
        {
        }
	}
}