using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Clc.Fields;
using Clc.Routes.Dto;

namespace Clc.Routes
{
    public interface IRouteAppService : IApplicationService
    {
        Task<List<RouteDto>> GetRoutesAsync(DateTime carryoutDate, string sorting);
        Task<List<RouteDto>> GetAuxRoutesAsync(DateTime carryoutDate, int workplaceId, string sorting);
        Task<List<RouteDto>> GetQueryRoutesAsync(DateTime carryoutDate, int depotId, string sorting);

        Task<RouteDto> Insert(RouteDto input);
        Task<RouteDto> Update(RouteDto input);
        Task Delete(int id);

        void SetActiveRouteCache(DateTime carryoutDate);
        void SetStatus(DateTime carryoutDate, int depotId, int routeId, string status);
        Task<(string, int)> Activate(List<int> ids, bool finger);
        Task<int> Close(List<int> ids);
        Task Back(int id, bool finger);

        Task<int> CreateFrom(DateTime carryoutDate, DateTime fromDate);
        Task<int> CreateFromPre(DateTime carryoutDate);

        #region Son Tables
        Task<List<RouteWorkerDto>> GetRouteWorkers(int id, string sorting);
        Task<RouteWorkerDto> InsertWorker(RouteWorkerDto input);
        Task<RouteWorkerDto> UpdateWorker(RouteWorkerDto input);
        Task DeleteWorker(int id);

        Task<List<RouteTaskDto>> GetRouteTasks(int id, string sorting);
        Task<RouteTaskDto> InsertTask(RouteTaskDto input);
        Task<RouteTaskDto> UpdateTask(RouteTaskDto input);

        Task<RouteTaskDto> UpdateTaskPrice(int id, int price);

        Task DeleteTask(int id);

        Task<List<RouteEventDto>> GetRouteEvents(int id);
        Task<List<RouteArticleDto>> GetRouteArticles(int id, string sorting);
        Task<List<RouteInBoxDto>> GetRouteInBoxes(int id, string sorting);
        Task<List<RouteOutBoxDto>> GetRouteOutBoxes(int id, string sorting);
        #endregion

        (Route, int) FindRouteForIdentify(int depotId, int workerId);
        void SetIdentifyTime(int taskId);
        void SetIdentifyEvent(int routeId, string outlet, string issuer);
    }
}
