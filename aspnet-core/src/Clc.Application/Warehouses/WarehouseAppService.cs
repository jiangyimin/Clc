using Abp.Authorization;
using Clc.Authorization;

namespace Clc.Users
{
    [AbpAuthorize(PermissionNames.Pages_TodayArrange)]
    public class WarehouseAppService : ClcAppServiceBase
    {
    }
}

