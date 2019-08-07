using System;
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

        public bool VerifyUnlockPassword(string password)
        {
            // Get DepotId and WorkCn
            try
            {
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string GetTodayString()
        {
            return DateTime.Now.Date.ToString("yyyy-MM-dd");
        }
    }
}