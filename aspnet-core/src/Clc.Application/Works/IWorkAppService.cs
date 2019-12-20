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
        MeDto GetMe();
        string GetMyPhoto(int id);
        string GetVehiclePhoto(int id);

        string GetToday();
        
        AffairWorkDto GetMyCheckinAffair();
        
        AffairWorkDto FindDutyAffair();
        AffairWorkDto FindAltDutyAffair();
        
        List<RouteCacheItem> GetCachedRoutes(int wpId, DateTime carryouDate, int depotId, int affairId);

        List<TempArticleDto> GetTempArticles(int affairId);

        (string, string) GetReportToManagers();

        TaskReportDto GetTaskReportData(); 

        void SetReportDate();

        Task<List<TemporaryTaskDto>> GetFeeTasks(DateTime dt, string sorting);
        Task<List<TemporaryTaskDto>> GetFeeTasks(FeeTaskSearchRequestDto input, string sorting);
        Task CaculateTasksPrice(DateTime dt);

        #region Agent / Workers
        string GetAgentString();
        Task SetAgent(int workerId);
        Task ResetAgent();

        List<ComboboxItemDto> GetLeaders();
        List<WorkplaceDto> GetDoors();

        SimpleWorkerDto GetWorkerByRfid(string rfid);

        #endregion

        #region Signin，Checkin，Confirm
        Task<List<SigninDto>> GetSigninsAsync(DateTime carryoutDate);
        (bool, string) SigninByRfid(string rfid);
        (bool, string) SigninByFinger(string finger);
        
        (bool, string) VerifyFinger(string finger, string workerCn);
        
        (bool, string) CheckinByFinger(string finger, int workerId, DateTime carryoutDate, int depotId, int affairId);
        (bool, string) CheckinByRfid(string rfid, DateTime carryoutDate, int depotId, int affairId);

        (bool, string) ConfirmByFinger(string finger, int workerId);

        #endregion Signin

        #region article

        RouteWorkerMatchResult MatchWorkerForArticle(bool isLend, int wpId, DateTime carryoutDate, int depotId, string rfid, int routeId);
        RouteWorkerMatchResult MatchWorkerForTakeTempArticle(DateTime carryoutDate, int depotId, int affairId, string rfid);
        RouteWorkerMatchResult MatchWorkerForStoreTempArticle(DateTime carryoutDate, int depotId, int affairId, string rfid);
        
        (string, RouteArticleCDto) MatchArticleForLend(string workerCn, string vehicleCn, string routeName, string articleTypeList, string rfid);
        (string, int) MatchArticleForReturn(string rfid);
        
        #endregion

        #region box

        RouteWorkerMatchResult MatchWorkerForBox(int wpId, DateTime carryoutDate, int depotId, int affairId, string rfid);
        
        (string, RouteBoxCDto) MatchInBox(DateTime carryoutDate, int affairId, int routeId, string rfid);
        (string, RouteBoxCDto) MatchOutBox(DateTime carryoutDate, int affairId, int routeId, string rfid);
        
        #endregion
    }
}
