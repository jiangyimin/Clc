using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Clc.Extensions;

namespace Clc.Works
{
    [AbpAuthorize]
    public class WorkAppService : ClcAppServiceBase, IWorkAppService
    {
        public WorkAppService()
        {
        }

        public Task<bool> VerifyUnlockPassword(string password)
        {
            // Get DepotId and WorkCn
            try
            {
                int depotId = AbpSession.GetDepotId();
                return Task.FromResult(true);
            }
            catch 
            {
                return Task.FromResult(false);
            }
       }        
    }
}