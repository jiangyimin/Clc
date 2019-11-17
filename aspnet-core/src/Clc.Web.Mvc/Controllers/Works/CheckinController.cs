using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Web.Models;
using Clc.Authorization;
using Clc.Controllers;
using System.Threading.Tasks;
using Clc.Affairs;
using Clc.Runtime.Cache;
using Clc.Fields;
using Clc.Works;
using Clc.Works.Dto;

namespace Clc.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Article, PermissionNames.Pages_Box, PermissionNames.Pages_Monitor, PermissionNames.Pages_Aux)]
    public class CheckinController : ClcControllerBase
    {
        public WorkManager WorkManager { get; set; } 
        private readonly IAffairAppService _affairAppService;
        private readonly IWorkAppService _workAppService;

        public CheckinController(IAffairAppService affairAppService,
            IWorkAppService workAppService)
        {
            _affairAppService = affairAppService;
            _workAppService = workAppService;
        }

        public ActionResult Index(MyAffairWorkDto affair)
        {
            if (!affair.Alt && affair.AffairId == 0) {
                var vm = _workAppService.FindDutyAffair();
                return View(vm);
            }
            return View(affair);
        }

        public ActionResult GetPhoto(string id)
        {
            Worker w = WorkManager.GetWorkerByRfid(id);
            if (w != null && w.Photo != null)
                return File(w.Photo, "image/jpg");

            return File(new byte[0], "image/jpg");
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