using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using Clc.Authorization;
using Clc.Fields;
using Clc.Fields.Dto;

namespace Clc.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Fields)]
    public class GasStationsController : ClcCrudController<GasStation, GasStationDto>
    {
        public GasStationsController(IRepository<GasStation> repository)
            :base(repository)
        {
        }
	}
}