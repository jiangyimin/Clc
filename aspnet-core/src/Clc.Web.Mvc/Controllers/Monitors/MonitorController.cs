using System;
using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Web.Models;
using Clc.Authorization;
using Clc.Controllers;
using Clc.Works;
using System.Threading.Tasks;
using Clc.ArticleRecords;
using Clc.Web.MessageHandlers;
using Clc.Monitors;

namespace Clc.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Monitor)]
    public class MonitorController : ClcControllerBase
    {
        private readonly IWorkAppService _workAppService;
        private readonly IMonitorAppService _monitorAppService;

        public MonitorController(IMonitorAppService monitorAppService, IWorkAppService workAppService)
        {
            _monitorAppService = monitorAppService;
            _workAppService = workAppService;
        }

        public ActionResult OpenDoor()
        {
            return View();
        }

        [DontWrapResult]
        public async Task<JsonResult> GridData()
        {
            var output = await _monitorAppService.GetDoorsAsync();
            return Json( new { rows = output });
        }

        [DontWrapResult]
        public async Task<JsonResult> GridDataRecord(int workplaceId)
        {
            var output = await _monitorAppService.GetRecordsAsync(workplaceId, GetPagedInput());
            return Json( new { total = output.TotalCount, rows = output.Items });
        }

    }
}