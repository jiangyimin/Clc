using System.Collections.Generic;
using Abp.Application.Services;
using Abp.Application.Services.Dto;

namespace Clc.Fields
{
    public interface IFieldAppService : IApplicationService
    {
        List<ComboboxItemDto> GetComboItems(string name);

        List<Workplace> GetWorkplaceItems(bool all = false);

        List<WorkerListItem> GetWorkerListItems(bool all = false);
        List<VehicleListItem> GetVehicleListItems(bool all = false);
    }
}
