using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.Domain.Services;
using Abp.Runtime.Caching;
using Clc.Fields.Cache;

namespace Clc.Fields
{
    /// <summary>
    /// Depot manager.
    /// Implements domain logic for depot.
    /// </summary>
    public class FieldProvider : IDomainService
    {
        public ICacheManager CacheManager { protected get; set; }

        private readonly IDepotCache _depotCache;

        public FieldProvider(
            IDepotCache depotCache
        )
        {
            _depotCache = depotCache;
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
    }
}