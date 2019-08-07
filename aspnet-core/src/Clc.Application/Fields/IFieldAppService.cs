using System.Collections.Generic;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Clc.Fields.Dto;

namespace Clc.Fields
{
    public interface IFieldAppService : IApplicationService
    {
        List<ComboboxItemDto> GetComboItems(string name);

        List<WorkplaceDto> GetWorkplaceItems(bool all = false);
    }
}
