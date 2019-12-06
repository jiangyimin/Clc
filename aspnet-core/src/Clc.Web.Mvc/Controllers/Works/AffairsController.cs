using System;
using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Web.Models;
using Clc.Authorization;
using Clc.Controllers;
using Clc.Affairs;
using System.Threading.Tasks;

namespace Clc.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Arrange, PermissionNames.Pages_Monitor)]
    public class AffairsController : ClcControllerBase
    {
        private readonly IAffairAppService _affairAppService;

        public AffairsController(IAffairAppService affairAppService)
        {
            _affairAppService = affairAppService;
       }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RQuery(int seld)
        {
            return View(seld);
        }

        
        [DontWrapResult]
        public async Task<JsonResult> GridData(DateTime carryoutDate)
        {
            var output = await _affairAppService.GetAffairsAsync(carryoutDate, GetSorting());
            return Json( new { rows = output });
        }

        [DontWrapResult]
        public async Task<JsonResult> QueryGridData(DateTime carryoutDate, int depotId)
        {
            var output = await _affairAppService.GetQueryAffairsAsync(carryoutDate, depotId, GetSorting());
            return Json( new { rows = output });
        }

        [DontWrapResult]
        public async Task<JsonResult> GridDataWorker(int id)
        {
            var output = await _affairAppService.GetAffairWorkersAsync(id);
            return Json( new { rows = output });
        }

        [DontWrapResult]
        public async Task<JsonResult> GridDataTask(int id)
        {
            var output = await _affairAppService.GetAffairTasksAsync(id, GetSorting());
            return Json( new { rows = output });
        }

        [DontWrapResult]
        public async Task<JsonResult> GridDataEvent(int id)
        {
            var output = await _affairAppService.GetAffairEventsAsync(id);
            return Json( new { rows = output });
        }
    }
}