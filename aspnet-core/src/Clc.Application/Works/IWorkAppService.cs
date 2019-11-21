using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
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
        string GetMyPhoto(int id);
        string GetVehiclePhoto(int id);

        string GetToday();
        
        AffairWorkDto GetMyCheckinAffair();
        
        AffairWorkDto FindDutyAffair();
        AffairWorkDto FindAltDutyAffair();
        
        List<RouteCacheItem> GetActiveRoutes(int wpId, DateTime carryouDate, int depotId, int affairId);

        string GetReportToManagers();

        #region Agent / Workers
        string GetAgentString();
        Task SetAgent(int workerId);
        Task ResetAgent();

        List<ComboboxItemDto> GetLeaders();
        List<WorkplaceDto> GetDoors();

        SimpleWorkerDto GetWorkerByRfid(string rfid);

        #endregion

        #region Signin and Checkin
        Task<List<SigninDto>> GetSigninsAsync(DateTime carryoutDate);
        (bool, string) SigninByRfid(string rfid);
        (bool, string) SigninByFinger(string finger);
        
        
        (bool, string) CheckinByFinger(string finger, int workerId, DateTime carryoutDate, int depotId, int affairId);
        (bool, string) CheckinByRfid(string rfid, DateTime carryoutDate, int depotId, int affairId);

        #endregion Signin

        #region article

        RouteWorkerMatchResult MatchWorkerForArticle(bool isLend, int wpId, DateTime carryoutDate, int depotId, int affairId, string rfid, int routeId);
        
        (string, RouteArticleCDto) MatchArticleForLend(string workerCn, string vehicleCn, string routeName, string articleTypeList, string rfid);
        (string, RouteArticleCDto) MatchArticleForReturn(string rfid);
        
        #endregion

        #region box

        RouteWorkerMatchResult MatchWorkerForBox(int wpId, DateTime carryoutDate, int depotId, int affairId, string rfid);
        
        (string, RouteBoxCDto) MatchInBox(DateTime carryoutDate, int affairId, int routeId, string rfid);
        (string, RouteBoxCDto) MatchOutBox(DateTime carryoutDate, int affairId, int routeId, string rfid);
        
        #endregion
    }
}
