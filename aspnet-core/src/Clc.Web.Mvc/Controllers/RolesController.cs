using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Web.Models;
using Clc.Authorization;
using Clc.Controllers;
using Clc.Roles;
using Clc.Roles.Dto;
using Clc.Web.Models.Roles;
using Clc.MultiTenancy;
using Clc.MultiTenancy.Dto;

namespace Clc.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Setup)]
    public class RolesController : ClcControllerBase
    {
        private readonly ITenantAppService _tenantAppService;
        private readonly IRoleAppService _roleAppService;

        public RolesController(ITenantAppService tenantAppService, IRoleAppService roleAppService)
        {
            _tenantAppService = tenantAppService;
            _roleAppService = roleAppService;
        }

        public async Task<IActionResult> Index()
        {
            var permissions = (await _roleAppService.GetAllPermissions()).Items; //PermissionManager.GetAllPermissions(false);            
            return View(permissions);
        }

       [DontWrapResult]
        public async Task<JsonResult> GridData()
        {
            var output = await _tenantAppService.GetAll(new PagedTenantResultRequestDto { MaxResultCount = int.MaxValue }); // Paging not implemented yet
            return Json( new { rows = output.Items });
        }

        [DontWrapResult]
        public async Task<JsonResult> GetTenantRoles(string id)     // where id = tenantName
        {
            var output = await _roleAppService.GetTenantRoles(id);
            return Json( new { rows = output.Items });
        }
    }
}
