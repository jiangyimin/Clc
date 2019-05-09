using Abp.Application.Services;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using Clc.Authorization;
using Clc.Types.Entities;
using Clc.Types.Dto;

namespace Clc.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Types)]
    public class WorkerTypesController : ClcCrudController<WorkerType, WorkerTypeDto>
    {
        public WorkerTypesController(IRepository<WorkerType> repository)
            : base(repository)
        {
        }
	}
}