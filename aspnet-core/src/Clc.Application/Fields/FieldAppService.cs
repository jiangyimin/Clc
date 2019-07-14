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