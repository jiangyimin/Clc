using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;

namespace Clc.Fields
{
    [AbpAuthorize]
    public class FieldAppService : ClcAppServiceBase, IFieldAppService
    {
        private readonly FieldProvider _fieldProvider;

        public FieldAppService(FieldProvider fieldProvider)
        {
            _fieldProvider = fieldProvider;
        }

        public Task<List<ComboboxItemDto>> GetComboItems(string name)
        {
            List<ComboboxItemDto> lst = _fieldProvider.GetComboItems(name);
            return Task.FromResult<List<ComboboxItemDto>>(lst);
        }        
    }
}