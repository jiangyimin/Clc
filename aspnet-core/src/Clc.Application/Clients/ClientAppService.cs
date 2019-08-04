using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;

namespace Clc.Clients
{
    [AbpAuthorize]
    public class ClientAppService : ClcAppServiceBase, IClientAppService
    {
        private readonly ClientProvider _clientProvider;

        public ClientAppService(ClientProvider clientProvider)
        {
            _clientProvider = clientProvider;
        }

        public Task<List<ComboboxItemDto>> GetComboItems(string name)
        {
            List<ComboboxItemDto> lst = _clientProvider.GetComboItems(name);
            return Task.FromResult<List<ComboboxItemDto>>(lst);
        }        
    }
}