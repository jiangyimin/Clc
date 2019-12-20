using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Web.Models;
using Clc.Authorization;
using Clc.Controllers;
using System.Threading.Tasks;
using System;
using Clc.Works;
using Clc.Works.Dto;

namespace Clc.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Statistic)]
    public class StatisticController : ClcControllerBase
    {
        private readonly IWorkAppService _workAppService;

        public StatisticController(IWorkAppService workAppService)
        {
            _workAppService = workAppService;
        }

        public ActionResult CheckTask()
        {
            return View();
        }

        [DontWrapResult]
        public async Task<JsonResult> GridDataTask(DateTime dt)
        {
            var output = await _workAppService.GetFeeTasks(dt, GetSorting());
            return Json( new { rows = output });
        }

        [DontWrapResult]
        public async Task<JsonResult> SearchTasks(FeeTaskSearchRequestDto input)
        {
            var output = await _workAppService.GetFeeTasks(input, GetSorting());
            return Json( new { rows = output });
        }
    }
}