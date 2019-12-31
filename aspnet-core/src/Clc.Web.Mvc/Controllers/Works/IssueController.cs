using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Web.Models;
using Abp.UI;
using Clc.Authorization;
using Clc.Controllers;
using Clc.Works;
using Clc.BoxRecords;
using Clc.Issues;
using Clc.Issues.Dto;
using Clc.DoorRecords;
using Abp.Domain.Uow;
using Clc.Web.MessageHandlers;

namespace Clc.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Arrange)]
    public class IssueController : ClcControllerBase
    {
        public WorkManager WorkManager { get; set; }
        private readonly IWorkAppService _workAppService;
        private readonly IIssueAppService _issueAppService;
        private readonly IDoorRecordAppService _doorRecordAppService;

        public IssueController(IWorkAppService workAppService, 
            IIssueAppService issueAppService, IDoorRecordAppService doorRecordAppService)
        {
            _workAppService = workAppService;
            _issueAppService = issueAppService;
            _doorRecordAppService = doorRecordAppService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [DontWrapResult]
        public async Task<JsonResult> GridData()
        {
            var output = await _issueAppService.GetIssuesAsync(GetOnlyPagedInput());
            return Json( new { total = output.TotalCount, rows = output.Items });
        }

        [HttpPost]
        [UnitOfWork]
        public async Task<JsonResult> ProcessIssueEmergDoor(int issueId, int doorId, string content, int leaderId, string leader)
        {
            try
            {
                await _issueAppService.ProcessIssue(issueId, $"发送给 {leader} 审批");
                await _doorRecordAppService.ProcessIssueEmergDoor(issueId, doorId, content, leaderId);
                var toUser = leader.Split()[0];
                var wp = WorkManager.GetWorkplace(doorId);
                var depot = WorkManager.GetDepot(wp.DepotId);
                var msg = string.Format("有来自{0}大队的应急开门（{1}）申请", depot.Name, wp.Name);
                WeixinUtils.SendMessage("App03", toUser, msg);

                return Json(new { result = "success", content = $"完成处理并发送通知给{leader}" });
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("操作失败", ex.Message);
            }
        }

        [HttpPost]
        public async Task<JsonResult> ProcessIssueReport(int issueId, string content, int depotId)
        {
            try
            {
                var toUsers = WorkManager.GetReportToManagers(depotId);
                await _issueAppService.ProcessIssue(issueId, $"发送给{toUsers}");
                WeixinUtils.SendMessage("App03", toUsers, content);

                return Json(new { result = "success", content = $"完成处理并发送通知给{toUsers}" });
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("操作失败", ex.Message);
            }
        }

    }
}