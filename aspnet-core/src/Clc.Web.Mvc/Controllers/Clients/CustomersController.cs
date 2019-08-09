using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using Clc.Authorization;
using Clc.Clients;
using Clc.Clients.Dto;

namespace Clc.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Clients)]
    public class CustomersController : ClcCrudController<Customer, CustomerDto>
    {
        public CustomersController(IRepository<Customer> repository)
            :base(repository)
        {
        }
	}
}