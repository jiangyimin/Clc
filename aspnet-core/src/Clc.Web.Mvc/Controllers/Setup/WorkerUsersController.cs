using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Clc.Authorization;
using Clc.Controllers;
using Clc.Users;
using Clc.Users.Dto;
using Abp.Web.Models;

namespace Clc.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Setup)]
    public class WorkerUsersController : ClcControllerBase
    {
        private readonly IUserAppService _userAppService;

        public WorkerUsersController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [DontWrapResult]
        public async Task<JsonResult> GridData()
        {
            var p = GetOnlyPagedInput();
            var input = new PagedUserResultRequestDto() { IsWorker = true };
            input.MaxResultCount = p.MaxResultCount;
            input.SkipCount = p.SkipCount;

            var output = await _userAppService.GetAll(input);
            return Json( new { total = output.TotalCount, rows = output.Items });
        }
    }
}
