using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using Abp.Web.Models;
using Clc.Authorization;
using Clc.Fields;
using Clc.Fields.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Clc.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Hrm, PermissionNames.Pages_Hrq)]
    public class WorkerFilesController : ClcCrudController<WorkerFile, WorkerFileDto>
    {
        private readonly IFieldAppService _fieldAppService;
        
        public WorkerFilesController(IRepository<WorkerFile> repository, IFieldAppService fieldAppService)
            :base(repository)
        {
            _fieldAppService = fieldAppService;
        }

        public ActionResult Hrq()
        {
            return View();
        }

        [HttpPost]
        [DontWrapResult]
        public async Task<JsonResult> GetPagedData(PagedFileResultRequestDto input)
        {
            var p = GetPagedInput();
            input.MaxResultCount = p.MaxResultCount;
            input.Sorting = p.Sorting;
            input.SkipCount = p.SkipCount;
            //input.DepotId = depotId;
            //input.PostId = postId;
            var output = await _fieldAppService.SearchFilePagedResult(input);
            return Json( new { total = output.TotalCount, rows = output.Items });
        }
	}
}