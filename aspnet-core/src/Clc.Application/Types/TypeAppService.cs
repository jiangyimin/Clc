using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Reflection.Extensions;
using Clc;
using Clc.Types;
using Clc.Types.Dto;

namespace Tbs.DomainModels
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