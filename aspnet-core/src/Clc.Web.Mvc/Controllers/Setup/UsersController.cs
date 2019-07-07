using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Clc.Authorization;
using Clc.Controllers;
using Clc.Users;
using Clc.Web.Models.Users;
using Clc.Users.Dto;
using Abp.Web.Models;

namespace Clc.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Setup)]
    public class UsersController : ClcControllerBase
    {
        private readonly IUserAppService _userAppService;

        public UsersController(IUserAppService userAppService)
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
            var output = await _userAppService.GetAll(new PagedUserResultRequestDto());
            return Json( new { rows = output.Items });
        }
    }
}
