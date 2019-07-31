using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using Clc.Authorization;
using Clc.Fields.Entities;
using Clc.Fields.Dto;

namespace Clc.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Fields)]
    public class WorkplacesController : ClcCrudController<Workplace, WorkplaceDto>
    {
        public WorkplacesController(IRepository<Workplace> repository)
            :base(repository)
        {
        }
	}
}