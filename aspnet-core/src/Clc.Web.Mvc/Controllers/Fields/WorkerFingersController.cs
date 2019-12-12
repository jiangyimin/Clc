using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Clc.Authorization;
using Clc.Controllers;
using Clc.Users;
using Clc.Users.Dto;
using Abp.Web.Models;
using Clc.Fields;
using Clc.Works;

namespace Clc.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Arrange)]
    public class WorkerFingersController : ClcControllerBase
    {
        private readonly IFieldAppService _fieldAppService;

        public WorkerFingersController(IFieldAppService fieldAppService)
        {
            _fieldAppService = fieldAppService;
        }

        public ActionResult Index()
        {
            if (_fieldAppService.AllowEditWorkerFinger())
                return View();
            else
                return Content("你不允许修改人员信息");
        }

        [DontWrapResult]
        public async Task<JsonResult> GridData()
        {
            var output = await _fieldAppService.GetWorkerFingersAsync(GetPagedInput());
            return Json( new { total = output.TotalCount, rows = output.Items });
        }
    }
}
