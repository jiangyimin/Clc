using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Clc.Affairs.Dto;
using Clc.Works.Dto;

namespace Clc.Affairs
{
    public interface IAffairAppService : IApplicationService
    {
        AffairDto GetAffair(int id);
        Task<List<AffairDto>> GetAffairsAsync(DateTime carryoutDate, string sorting);
        Task<List<AffairDto>> GetQueryAffairsAsync(DateTime carryoutDate, int depotId, string sorting);

        Task<(string, int)> Activate(List<int> ids, bool finger);
        Task SetActiveAffairCache(DateTime carryoutDate);

        Task Back(int id, bool finger);

        Task<int> CreateFrom(DateTime carryoutDate, DateTime fromDate);

        Task<AffairDto> Insert(AffairDto input);
        Task<AffairDto> Update(AffairDto input);
        Task Delete(int id);


        #region Son Tables
        Task<List<AffairWorkerDto>> GetAffairWorkersAsync(int id);
        List<AffairWorkerDto> GetAffairWorkersSync(int id);
        Task<AffairWorkerDto> InsertWorker(AffairWorkerDto input);
        Task<AffairWorkerDto> UpdateWorker(AffairWorkerDto input);
        Task DeleteWorker(int id);

        Task<List<AffairTaskDto>> GetAffairTasksAsync(int id, string sorting);
        Task<AffairTaskDto> InsertTask(AffairTaskDto input);
        Task<AffairTaskDto> UpdateTask(AffairTaskDto input);
        void SetTaskTime(int id, bool isStart);
        Task DeleteTask(int id);

        Task<List<AffairEventDto>> GetAffairEventsAsync(int id);

        Task<AffairEventDto> InsertEvent(int affairId, string name, string desc, string issurer);

        Task<AffairEventDto> InsertTempArticle(string style, int affairId, List<RouteArticleCDto> articles, string workers);
        #endregion
    }
}
