using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;

namespace Clc.Clients
{
    public interface IClientAppService : IApplicationService
    {
        Task<List<ComboboxItemDto>> GetComboItems(string name); 
    }
}
