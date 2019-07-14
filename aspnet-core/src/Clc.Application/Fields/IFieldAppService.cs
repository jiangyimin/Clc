using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;

namespace Clc.Fields
{
    public interface IFieldAppService : IApplicationService
    {
        Task<List<ComboboxItemDto>> GetComboItems(string name); 
    }
}
