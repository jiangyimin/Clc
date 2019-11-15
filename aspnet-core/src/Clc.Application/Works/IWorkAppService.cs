using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Clc.Fields;
using Clc.Fields.Dto;
using Clc.Routes;
using Clc.Works.Dto;

namespace Clc.Works
{
    public interface IWorkAppService : IApplicationService
    {
        bool VerifyUnlockPassword(string password);
        bool AllowCardWhenCheckin();   
        (string, string) GetMe();
        MyAffairWorkDto GetMyAffairWork();
        
        List<RouteCacheItem> GetActiveRoutes(DateTime carryouDate, int depotId, int affairId);

        string GetReportToManagers();

        #region Agent / Workers
        string GetAgentString();
        Task SetAgent(int workerId);
        Task ResetAgent();

        List<ComboboxItemDto> GetLeaders();
        List<WorkplaceDto> GetDoors();

        #endregion

        #region Signin and Checkin
        Task<List<SigninDto>> GetSigninsAsync(DateTime carryoutDate);
        (bool, string) SigninByRfid(string rfid);
        (bool, string) SigninByFinger(string finger);
        
        
        (bool, string) CheckinByFinger(string finger, int workerId, DateTime carryoutDate, int depotId, int affairId);
        (bool, string) CheckinByRfid(string rfid, DateTime carryoutDate, int depotId, int affairId);

        #endregion Signin

        #region article

        // style = 0 为领物
        RouteWorkerMatchResult MatchWorkerForArticle(bool isLend, DateTime carryoutDate, int depotId, int affairId, string rfid);
        
        (string, RouteArticleCDto) MatchArticleForLend(string workerCn, string vehicleCn, string routeName, string articleTypeList, string rfid);
        (string, RouteArticleCDto) MatchArticleForReturn(string rfid);
        
        #endregion

        #region box

        RouteWorkerMatchResult MatchWorkerForBox(DateTime carryoutDate, int depotId, int affairId, string rfid);
        
        (string, RouteBoxCDto) MatchInBox(DateTime carryoutDate, int affairId, int routeId, string rfid);
        (string, RouteBoxCDto) MatchOutBox(DateTime carryoutDate, int affairId, int routeId, string rfid);
        
        #endregion
    }
}
