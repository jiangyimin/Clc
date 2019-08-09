using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Clc.Runtime.Cache;
using Clc.Fields;
using Clc.Fields.Dto;
using Clc.Types;

namespace Clc.Fields
{
    [AbpAuthorize]
    public class FieldAppService : ClcAppServiceBase, IFieldAppService
    {
        private readonly IDepotCache _depotCache;
        private readonly IWorkplaceCache _workplaceCache;
        private readonly IWorkerCache _workerCache;
        private readonly IWorkRoleCache _workRoleCache;

        public FieldAppService(IDepotCache depotCache,
            IWorkplaceCache workplaceCache,
            IWorkerCache workerCache,
            IWorkRoleCache workRoleCache)
        {
            _depotCache = depotCache;
            _workplaceCache = workplaceCache;
            _workerCache = workerCache;
            _workRoleCache = workRoleCache;
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

        public List<Workplace> GetWorkplaceItems(bool all)
        {
            if (all)
            {
                return _workplaceCache.GetList();
            }
            else
            {
                int depotId = WorkManager.GetWorkerDepotId(GetCurrentUserWorkerIdAsync().Result);
                return _workplaceCache.GetList().FindAll(x => x.DepotId == depotId);
            }
        }

        public List<WorkerListItem> GetWorkerListItems(bool all)        
        {
            if (all)
            {
                return ObjectMapper.Map<List<WorkerListItem>>(_workplaceCache.GetList());
            }
            else
            {
                int depotId = WorkManager.GetWorkerDepotId(GetCurrentUserWorkerIdAsync().Result);
                var lst = _workerCache.GetList().FindAll(x => x.DepotId == depotId || (x.LoanDepotId.HasValue && x.LoanDepotId.Value == depotId));
                lst.Sort( (a, b) => a.Cn.CompareTo(b.Cn) );
                return ObjectMapper.Map<List<WorkerListItem>>(lst);
            }
        }

        public List<WorkRole> GetWorkRoleItems(int workplaceId)        
        {
            List<string> targetRoles = new List<string>(_workplaceCache[workplaceId].WorkRoles.Split('|', ','));
            var all = _workRoleCache.GetList();
            return all.FindAll(x => targetRoles.Contains(x.Name));
        }
 
    }
}