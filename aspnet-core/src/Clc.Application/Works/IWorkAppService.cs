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

        string GetReportToManagers();

        #region Signin
        Task<List<SigninDto>> GetSigninsAsync(DateTime carryoutDate);

        string SigninByRfid(string rfid);
        
        #endregion Signin

        #region article
        Task<List<RouteCDto>> GetRoutesForArticleAsync(DateTime carryouDate, int affairId);

        RouteWorkerMatchResult MatchWorkerForLend(DateTime carryoutDate, int affairId, string rfid);
        RouteWorkerMatchResult MatchWorkerForReturn(DateTime carryoutDate, int affairId, string rfid);
        
        (string, RouteArticleCDto) MatchArticleForLend(string workerCn, string vehicleCn, string routeName, string articleTypeList, string rfid);
        (string, RouteArticleCDto) MatchArticleForReturn(string rfid);
        
        #endregion

        #region box
        Task<List<RouteCDto>> GetRoutesForBoxAsync(DateTime carryouDate, int affairId);

        RouteWorkerMatchResult MatchWorkerForInBox(DateTime carryoutDate, int affairId, string rfid);
        RouteWorkerMatchResult MatchWorkerForOutBox(DateTime carryoutDate, int affairId, string rfid);
        
        (string, RouteBoxCDto) MatchInBox(DateTime carryoutDate, int affairId, int routeId, string rfid);
        (string, RouteBoxCDto) MatchOutBox(DateTime carryoutDate, int affairId, int routeId, string rfid);
        
        #endregion
    }
}
