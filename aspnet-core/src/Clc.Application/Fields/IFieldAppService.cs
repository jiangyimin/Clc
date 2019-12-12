using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Clc.Fields.Dto;

namespace Clc.Fields
{
    public interface IFieldAppService : IApplicationService
    {
        List<ComboboxItemDto> GetComboItems(string name);

        List<Workplace> GetWorkplaceItems(bool justVault = false);

        List<WorkerCacheItem> GetWorkerCacheItems(bool all = false);
        List<ComboboxItemDto> GetWorkerItemsByWorkRole(int workRoleId);
        List<VehicleListItem> GetVehicleListItems(bool all = false);

        Task<PagedResultDto<WorkerFingerDto>> GetWorkerFingersAsync(PagedAndSortedResultRequestDto input);
        Task<WorkerFingerDto> UpdateWorkerFingerAsync(WorkerFingerDto input);

        // WorkerFile and Asset
        Task<PagedResultDto<AssetDto>> SearchAssetPagedResult(PagedAssetResultRequestDto input);
        Task<PagedResultDto<WorkerFileDto>> SearchFilePagedResult(PagedFileResultRequestDto input);
        List<ComboboxItemDto> GetWorkerComboItems(int depotId);

        // WorkerFinger
        bool AllowEditWorkerFinger();

        // Asset
    }
}
