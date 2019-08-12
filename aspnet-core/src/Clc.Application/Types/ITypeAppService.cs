using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;

namespace Clc.Types
{
    public interface ITypeAppService : IApplicationService
    {
        Task<List<ComboboxItemDto>> GetComboItems(string typeName);
        Task<List<ComboboxItemDto>> GetDutyCategories();

        List<WorkRole> GetWorkRoleItems(int workplaceId);

        List<WorkRole> GetRouteWorkRoleItems(int routeTypeId);
    }
}
