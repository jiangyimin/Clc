using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Clc.Routes.Dto;
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
        
        #endregion Signin

        #region Routes for article box
        Task<List<RouteCDto>> GetRoutesForLendAsync(DateTime carryouDate, int affairId);
        Task<List<RouteCDto>> GetRoutesForReturnAsync(DateTime carryouDate, int affairId);

        RouteWorkerMatchResult MatchWorkerForLend(DateTime carryoutDate, int affairId, string rfid);
        
        #endregion
    }
}
