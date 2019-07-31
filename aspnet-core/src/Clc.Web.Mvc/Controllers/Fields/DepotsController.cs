using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using Clc.Authorization;
using Clc.Fields.Entities;
using Clc.Fields.Dto;

namespace Clc.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Fields)]
    public class DepotsController : ClcCrudController<Depot, DepotDto>
    {
        public DepotsController(IRepository<Depot> repository)
            :base(repository)
        {
        }

	}
}