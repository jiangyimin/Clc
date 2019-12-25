using System.Collections.Generic;
using Abp.Application.Services;
using Abp.Application.Services.Dto;

namespace Clc.Clients
{
    public interface IClientAppService : IApplicationService
    {
        List<ComboboxItemDto> GetComboItems(string name);

        List<ComboboxItemDto> GetBoxes(int outletId);
    }
}
