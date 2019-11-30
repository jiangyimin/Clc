﻿using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using Abp.Web.Models;
using Clc.Authorization;
using Clc.Fields;
using Clc.Fields.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Clc.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Hrm)]
    public class AssetsController : ClcCrudController<Asset, AssetDto>
    {
        private readonly IFieldAppService _fieldAppService;
        
        public AssetsController(IRepository<Asset> repository, IFieldAppService fieldAppService)
            :base(repository)
        {
            _fieldAppService = fieldAppService;
        }

        [DontWrapResult]
        public async Task<JsonResult> GetFilePagedData(int id)
        {
            var output = await _fieldAppService.GetPagedResult(id, GetPagedInput());
            return Json( new { total = output.TotalCount, rows = output.Items });
        }

        public ActionResult Query()
        {
            return View();
        }
	}
}