using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace Clc.Controllers
{
    public abstract class ClcControllerBase: AbpController
    {
        protected ClcControllerBase()
        {
            LocalizationSourceName = ClcConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        protected PagedAndSortedResultRequestDto GetPagedInput()
        {
            PagedAndSortedResultRequestDto input = new PagedAndSortedResultRequestDto();
            input.Sorting = GetSorting();
            input.MaxResultCount = int.Parse(Request.Form["rows"]);
            input.SkipCount = (int.Parse(Request.Form["page"]) - 1) * input.MaxResultCount;
            return input;
        }

        protected PagedResultRequestDto GetOnlyPagedInput()
        {
            PagedAndSortedResultRequestDto input = new PagedAndSortedResultRequestDto();
            // input.Sorting = GetSorting();
            input.MaxResultCount = int.Parse(Request.Form["rows"]);
            input.SkipCount = (int.Parse(Request.Form["page"]) - 1) * input.MaxResultCount;
            return input;
        }

        protected string GetSorting()
        {
            return $"{Request.Form["sort"]} {Request.Form["order"]}";
        }

        protected string AbsoluteUri()
        {
            return $"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}";
        }
    }
}
