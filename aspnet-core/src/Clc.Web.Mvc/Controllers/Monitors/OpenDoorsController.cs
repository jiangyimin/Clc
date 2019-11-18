using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Web.Models;
using Clc.Authorization;
using Clc.Controllers;
using Clc.Works;
using System.Threading.Tasks;
using Clc.DoorRecords;
using System;

namespace Clc.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Monitor)]
    public class OpenDoorController : ClcControllerBase
    {
        private readonly IWorkAppService _workAppService;
        private readonly IDoorRecordAppService _doorRecordAppService;

        public OpenDoorController(IDoorRecordAppService doorRecordAppService, IWorkAppService workAppService)
        {
            _doorRecordAppService = doorRecordAppService;
            _workAppService = workAppService;
        }

        public ActionResult EmergOpenDoor()
        {
            return View();
        }
        public ActionResult AskOpenDoor()
        {
            return View();
        }

        [DontWrapResult]
        public async Task<JsonResult> GridDataDoor()
        {
            var output = await _doorRecordAppService.GetDoorsAsync();
            return Json( new { rows = output });
        }

        [DontWrapResult]
        public async Task<JsonResult> GridDataAskDoor(DateTime day)
        {
            var output = await _doorRecordAppService.GetAskDoorsAsync(day);
            return Json( new { rows = output });
        }

        [DontWrapResult]
        public async Task<JsonResult> GridDataEmergDoor(DateTime day)
        {
            var output = await _doorRecordAppService.GetEmergDoorsAsync(day);
            return Json( new { rows = output });
        }

        [DontWrapResult]
        public async Task<JsonResult> GridDataAskDoorRecord(int workplaceId)
        {
            var output = await _doorRecordAppService.GetAskDoorRecordsAsync(workplaceId, GetPagedInput());
            return Json( new { total = output.TotalCount, rows = output.Items });
        }

        [DontWrapResult]
        public async Task<JsonResult> GridDataEmergDoorRecord(int workplaceId)
        {
            var output = await _doorRecordAppService.GetEmergDoorRecordsAsync(workplaceId, GetPagedInput());
            return Json( new { total = output.TotalCount, rows = output.Items });
        }

    }
}