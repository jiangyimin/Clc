using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using Clc.Authorization;
using Clc.Fields;
using Clc.Fields.Dto;

namespace Clc.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Fields)]
    public class ArticleTypeBindsController : ClcCrudController<ArticleTypeBind, ArticleTypeBindDto>
    {
        public ArticleTypeBindsController(IRepository<ArticleTypeBind> repository)
            :base(repository)
        {
        }
	}
}