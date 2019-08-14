using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Clc.Works.Dto;

namespace Clc.Works
{
    public interface IWorkAppService : IApplicationService
    {
        bool VerifyUnlockPassword(string password);
        string GetTodayString();
        DateTime getNow();
        MyWorkDto GetMyWork();

        #region Signin
        Task<List<SigninDto>> GetSigninsAsync(DateTime carryoutDate);

        string SigninByRfid(string rfid);
        string SigninByWx(string cn, float lat, float lon);
        
        #endregion Signin
    }
}
