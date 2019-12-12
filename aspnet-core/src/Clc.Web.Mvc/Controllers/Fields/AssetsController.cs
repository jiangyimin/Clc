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
    public class AssetsController : ClcCrudController<Asset, AssetDto>
    {
        private readonly IFieldAppService _fieldAppService;
        
        public AssetsController(IRepository<Asset> repository, IFieldAppService fieldAppService)
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
        public async Task<JsonResult> GetPagedData(PagedAssetResultRequestDto input)
        {
            var p = GetPagedInput();
            input.MaxResultCount = p.MaxResultCount;
            input.Sorting = p.Sorting;
            input.SkipCount = p.SkipCount;
            var output = await _fieldAppService.SearchAssetPagedResult(input);
            return Json( new { total = output.TotalCount, rows = output.Items });
        }

	}
}