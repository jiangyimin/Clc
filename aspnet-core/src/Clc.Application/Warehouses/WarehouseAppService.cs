using Abp.Authorization;
using Clc.Authorization;

namespace Clc.Warehouses
{
    [AbpAuthorize(PermissionNames.Pages_TodayArrange)]
    public class WarehouseAppService : ClcAppServiceBase, IWarehouseAppService
    {
        
    }
}

