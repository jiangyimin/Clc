using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using Clc.Authorization;
using Clc.Clients.Entities;
using Clc.Clients.Dto;

namespace Clc.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Clients)]
    public class BoxesController : ClcCrudController<Box, BoxDto>
    {
        public BoxesController(IRepository<Box> repository)
            :base(repository)
        {
        }
	}
}