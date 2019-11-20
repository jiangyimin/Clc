using System;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using Abp.Web.Models;
using Clc.Authorization;
using Clc.Fields;
using Clc.Fields.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Clc.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Fields)]
    public class WorkersController : ClcCrudController<Worker, WorkerDto>
    {
        public WorkersController(IRepository<Worker> repository)
            :base(repository)
        {
        }
	}
}