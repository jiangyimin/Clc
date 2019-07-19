using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Runtime.Caching;
using Clc.Fields.Cache;

namespace Clc.Fields
{
    /// <summary>
    /// Depot manager.
    /// Implements domain logic for depot.
    /// </summary>
    public class FieldProvider : ITransientDependency
    {        
        private readonly IDepotCache _depotCache;
        private readonly IWorkerCache _workerCache;

        public FieldProvider(
            IDepotCache depotCache,
            IWorkerCache workerCache)
        {
            _depotCache = depotCache;
            _workerCache = workerCache;
        }
        
        public List<ComboboxItemDto> GetComboItems(string name)
        {
            var lst = new List<ComboboxItemDto>();
            switch (name) 
            {
                case "Depot":
                    foreach (Depot t in _depotCache.GetList())
                        lst.Add(new ComboboxItemDto { Value = t.Id.ToString(), DisplayText = t.Name });
                    break;
                default:
                    break;
            }
            return lst;
        }

        public Depot GetDepotById(int id)
        {
            return _depotCache.GetById(id);
        }
        public string GetDepotNameById(int id)
        {
            return _depotCache.GetById(id).Name;
        }
        
        public Worker GetWorkerByCn(string workerCn)
        {
            return _workerCache.GetByCn(workerCn);
        }
    }
}