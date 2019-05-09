using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Clc.MultiTenancy.Dto;

namespace Clc.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

