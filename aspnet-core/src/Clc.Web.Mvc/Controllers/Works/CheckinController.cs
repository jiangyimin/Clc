using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Web.Models;
using Clc.Authorization;
using Clc.Controllers;
using System.Threading.Tasks;
using Clc.Affairs;
using Clc.Runtime.Cache;
using Clc.Fields;

namespace Clc.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Article, PermissionNames.Pages_Box, PermissionNames.Pages_Monitor, PermissionNames.Pages_Aux)]
    public class CheckinController : ClcControllerBase
    {
        private readonly IAffairAppService _affairAppService;
        private readonly IWorkerCache _workerCache;

        public CheckinController(IAffairAppService affairAppService,
            IWorkerCache workerCache)
        {
            _affairAppService = affairAppService;
            _workerCache = workerCache;
        }

        public ActionResult Index()
        {
            return View();
        }

        [DontWrapResult]
        public async Task<JsonResult> GridDataWorker(int id)
        {
            var output = await _affairAppService.GetAffairWorkersAsync(id);
            return Json(new { rows = output });
        }

        [DontWrapResult]
        public async Task<JsonResult> GridDataTask(int id)
        {
            var output = await _affairAppService.GetAffairTasksAsync(id, GetSorting());
            return Json(new { rows = output });
        }
    }
}