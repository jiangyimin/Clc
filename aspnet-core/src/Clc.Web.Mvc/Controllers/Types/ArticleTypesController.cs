using Abp.Application.Services;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using Clc.Authorization;
using Clc.Types;
using Clc.Types.Dto;

namespace Clc.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Types)]
    public class ArticleTypesController : ClcCrudController<ArticleType, ArticleTypeDto>
    {
        public ArticleTypesController(IRepository<ArticleType> repository)
            : base(repository)
        {
        }
	}
}