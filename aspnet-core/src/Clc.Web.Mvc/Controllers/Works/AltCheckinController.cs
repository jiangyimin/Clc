using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Clc.Authorization;
using Clc.Controllers;
using Clc.Works;
using Clc.Configuration;
using Clc.Works.Dto;

namespace Clc.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Article)]
    public class AltCheckinController : ClcControllerBase
    {
        public WorkManager WorkManger { get; set; }
        private readonly IWorkAppService _workAppService;

        public AltCheckinController(IWorkAppService workAppService)
        {
            _workAppService = workAppService;
        }

        public ActionResult Index()
        {           
            var affair = _workAppService.FindAltDutyAffair();
            return RedirectToAction("Index", "Checkin", new {
                AltCheck = affair.AltCheck,
                Today = affair.Today,
                DepotId = affair.DepotId,
                AffairId = affair.AffairId,
                Content = affair.Content,
                WorkplaceName = affair.WorkplaceName,
                StartTime = affair.StartTime,
                EndTIme = affair.EndTime
            });
        }

    }
}