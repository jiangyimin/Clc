using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Clc.Runtime.Cache;
using Clc.Fields.Entities;
using Clc.Fields.Dto;

namespace Clc.Fields
{
    [AbpAuthorize]
    public class FieldAppService : ClcAppServiceBase, IFieldAppService
    {
        private readonly IDepotCache _depotCache;
        private readonly IWorkplaceCache _workplaceCache;

        public FieldAppService(IDepotCache depotCache,
            IWorkplaceCache workplaceCache)
        {
            _depotCache = depotCache;
            _workplaceCache = workplaceCache;
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
                case "DepotWithCn":
                    foreach (Depot t in _depotCache.GetList())
                        lst.Add(new ComboboxItemDto { Value = t.Id.ToString(), DisplayText = string.Format("{0} {1}", t.Cn, t.Name) });
                    break;
                default:
                    break;
            }
            return lst;
        }

        public List<WorkplaceDto> GetWorkplaceItems(bool all)
        {
            if (all)
            {
                return ObjectMapper.Map<List<WorkplaceDto>>(_workplaceCache.GetList());
            }
            else
            {
                int depotId = WorkManager.GetWorkerDepotId(GetCurrentUserWorkerIdAsync().Result);
                return ObjectMapper.Map<List<WorkplaceDto>>(_workplaceCache.GetList().FindAll(x => x.DepotId == depotId));
            }
        }        
    }
}