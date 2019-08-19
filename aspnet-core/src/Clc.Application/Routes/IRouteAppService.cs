using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Clc.Routes.Dto;

namespace Clc.Routes
{
    public interface IRouteAppService : IApplicationService
    {
        Task<List<RouteDto>> GetRoutesAsync(DateTime carryouDate, string sorting);
        Task<RouteDto> Insert(RouteDto input);
        Task<RouteDto> Update(RouteDto input);
        Task Delete(int id);

        Task<int> Activate(List<int> ids);
        Task<int> Close(List<int> ids);
        Task Back(int id);

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
        Task DeleteTask(int id);

        Task<List<RouteEventDto>> GetRouteEvents(int id, string sorting);
        Task<List<RouteArticleDto>> GetRouteArticles(int id, string sorting);
        Task<List<RouteInBoxDto>> GetRouteInBoxes(int id, string sorting);
        Task<List<RouteOutBoxDto>> GetRouteOutBoxes(int id, string sorting);
        #endregion
    }
}