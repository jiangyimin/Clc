using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Clc.Types.Dto;

namespace Tbs.DomainModels
{
    public interface ITypeAppService : IApplicationService
    {
        Task<List<ComboboxItemDto>> GetComboItems(string typeName);
        Task<List<ComboboxItemDto>> GetDutyCategories();

        Task<List<string>> GetDuties(string category);
 
    }
}
