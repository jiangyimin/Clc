using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;

namespace Clc.Types
{
    [AbpAuthorize]
    public class TypeAppService : ClcAppServiceBase, ITypeAppService
    {
        private readonly DutyProvider _dutyProvider;
        private readonly TypeProvider _typeProvider;

        public TypeAppService(DutyProvider dutyProvider, TypeProvider typeProvider)
        {
            _dutyProvider = dutyProvider;
            _typeProvider = typeProvider;
        }

        public Task<List<ComboboxItemDto>> GetComboItems(string typeName)
        {
            List<ComboboxItemDto> lst = _typeProvider.GetComboItems(typeName);
            return Task.FromResult<List<ComboboxItemDto>>(lst);
        }
        
        public Task<List<ComboboxItemDto>> GetDutyCategories()
        {
            List<ComboboxItemDto> lst = new List<ComboboxItemDto>();
            foreach (var c in _dutyProvider.GetDutyCategory()) 
            {
                lst.Add(new ComboboxItemDto() { Value = c, DisplayText = c});
            }
            return Task.FromResult<List<ComboboxItemDto>>(lst);            
        }
        
        //public Task<List<string>> GetDuties(string category)
        //{
            //List<string> lst = new List<string>();
            //foreach (var c in _dutyProvider.GetDuties(category)) 
            //{
                //lst.Add(c);
            //}
            //return Task.FromResult<List<string>>(lst);            
        //}
        
    }

}