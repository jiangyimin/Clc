using Abp.Authorization;
using Clc.Authorization;

namespace Clc.Warehouses
{
    [AbpAuthorize(PermissionNames.Pages_Arrange)]
    public class AffairAppService : ClcAppServiceBase, IAffairAppService
    {
        
    }
}

