using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Clc.PreRoutes.Dto;

namespace Clc.PreRoutes
{
    public interface IPreRouteAppService : IApplicationService
    {
        Task<List<PreRouteDto>> GetPreRoutesAsync(string sorting);

        Task<PreRouteDto> Insert(PreRouteDto input);
        Task<PreRouteDto> Update(PreRouteDto input);
        Task Delete(int id);


        #region Son Tables
        Task<List<PreRouteWorkerDto>> GetPreRouteWorkers(int id, string sorting);
        Task<PreRouteWorkerDto> InsertWorker(PreRouteWorkerDto input);
        Task<PreRouteWorkerDto> UpdateWorker(PreRouteWorkerDto input);
        Task DeleteWorker(int id);

        Task<List<PreRouteTaskDto>> GetPreRouteTasks(int id, string sorting);
        Task<PreRouteTaskDto> InsertTask(PreRouteTaskDto input);
        Task<PreRouteTaskDto> UpdateTask(PreRouteTaskDto input);
        Task DeleteTask(int id);

        Task<List<PreVehicleWorkerDto>> GetPreVehicleWorkers(int id, string sorting);
        Task<PreVehicleWorkerDto> InsertVehicleWorker(PreVehicleWorkerDto input);
        Task<PreVehicleWorkerDto> UpdateVehicleWorker(PreVehicleWorkerDto input);
        Task DeleteVehicleWorker(int id);
        #endregion
    }
}
