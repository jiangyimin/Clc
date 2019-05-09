using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Runtime.Caching;
using Clc.Types.Cache;
using Clc.Types.Entities;

namespace Clc.Types
{
    /// <summary>
    /// Depot manager.
    /// Implements Typs Manager.
    /// </summary>
    public class TypeProvider : ITransientDependency
    {        
        // public ICacheManager CacheManager { protected get; set; }
        private readonly ITaskTypeCache _taskTypeCache;
        private readonly IWorkerTypeCache _workerTypeCache;


        public TypeProvider(
            TaskTypeCache taskTypeCache,
            WorkerTypeCache workerTypeCache )
        {
            _taskTypeCache = taskTypeCache;
            _workerTypeCache = workerTypeCache;
        }
        
        public List<ComboboxItemDto> GetComboItems(string tableName)
        {
            var lst = new List<ComboboxItemDto>();
            switch (tableName) 
            {
                case "WorkerType":
                    foreach (WorkerType t in _workerTypeCache.GetList())
                        lst.Add(new ComboboxItemDto { Value = t.Id.ToString(), DisplayText = t.Name });
                    break;
                case "TaskType":
                    foreach (TaskType t in _taskTypeCache.GetList())
                        lst.Add(new ComboboxItemDto { Value = t.Id.ToString(), DisplayText = t.Name });
                    break;
                default:
                    break;
            }
            return lst;
        }
    }
}
 