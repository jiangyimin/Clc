using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Clc.Affairs.Dto;

namespace Clc.Affairs
{
    public interface IAffairAppService : IApplicationService
    {
        Task<List<AffairDto>> GetAffairsAsync(DateTime carryoutDate, string sorting);
        Task Activate(int id, bool active);
        Task<int> CreateFrom(DateTime carryoutDate, DateTime fromDate);

         Task<AffairDto> Insert(AffairDto input);
        Task<AffairDto> Update(AffairDto input);
        Task Delete(int id);


        #region Son Tables
        Task<List<AffairWorkerDto>> GetAffairWorkers(int id, string sorting);
        Task<AffairWorkerDto> InsertWorker(AffairWorkerDto input);
        Task<AffairWorkerDto> UpdateWorker(AffairWorkerDto input);
        Task DeleteWorker(int id);

        Task<List<AffairTaskDto>> GetAffairTasks(int id, string sorting);
        Task<AffairTaskDto> InsertTask(AffairTaskDto input);
        Task<AffairTaskDto> UpdateTask(AffairTaskDto input);
        Task DeleteTask(int id);

        Task<List<AffairEventDto>> GetAffairEvents(int id, string sorting);

        #endregion
    }
}
