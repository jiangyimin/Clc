using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq;
using Clc.Fields.Dto;
using Clc.Runtime.Cache;

namespace Clc.Fields
{
    [AbpAuthorize]
    public class FieldAppService : ClcAppServiceBase, IFieldAppService
    {
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        private readonly IDepotCache _depotCache;
        private readonly IWorkplaceCache _workplaceCache;
        private readonly IWorkerCache _workerCache;
        private readonly IVehicleCache _vehicleCache;
        private readonly IPostCache _postCache;

        private readonly IRepository<Worker> _workerRepository;
        private readonly IRepository<WorkerFile> _workerFileRepository;

        public FieldAppService(IDepotCache depotCache,
            IWorkplaceCache workplaceCache,
            IWorkerCache workerCache,
            IVehicleCache vehicleCache,
            IPostCache postCache,
            IRepository<Worker> workerRepository, 
            IRepository<WorkerFile> workerFileRepository)
        {
            _depotCache = depotCache;
            _workplaceCache = workplaceCache;
            _workerCache = workerCache;
            _vehicleCache = vehicleCache;
            _postCache = postCache;
            _workerRepository = workerRepository;
            _workerFileRepository = workerFileRepository;
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
                int depotId = _workerCache[GetCurrentUserWorkerIdAsync().Result].DepotId;
                return _workplaceCache.GetList().FindAll(x => x.DepotId == depotId);
            }
        }

        public List<WorkerListItem> GetWorkerListItems(bool all)        
        {
            if (all)
            {
                return ObjectMapper.Map<List<WorkerListItem>>(_workerCache.GetList());
            }
            else
            {
                int depotId = _workerCache[GetCurrentUserWorkerIdAsync().Result].DepotId;
                var lst = _workerCache.GetList().FindAll(x => x.DepotId == depotId || (x.LoanDepotId.HasValue && x.LoanDepotId.Value == depotId));
                lst.Sort( (a, b) => a.Cn.CompareTo(b.Cn) );

                var list = ObjectMapper.Map<List<WorkerListItem>>(lst);
                list.ForEach(x => x.PostName = _postCache[x.PostId].Name);
                return list;
            }
        }

        public List<VehicleListItem> GetVehicleListItems(bool all)        
        {
            if (all)
            {
                return ObjectMapper.Map<List<VehicleListItem>>(_vehicleCache.GetList());
            }
            else
            {
                int depotId = _workerCache[GetCurrentUserWorkerIdAsync().Result].DepotId;
                var lst = _vehicleCache.GetList().FindAll(x => x.DepotId == depotId);
                lst.Sort( (a, b) => a.Cn.CompareTo(b.Cn) );
                return ObjectMapper.Map<List<VehicleListItem>>(lst);
            }
        }

        public async Task<PagedResultDto<WorkerFileDto>> GetPagedResult(int depotId, PagedAndSortedResultRequestDto input)
        {
            var query = _workerFileRepository.GetAllIncluding(x => x.Worker)
                .Where(x => x.Worker.DepotId == depotId);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            if (!string.IsNullOrWhiteSpace(input.Sorting))
                query = query.OrderBy(input.Sorting);                               // Applying Sorting
            query = query.Skip(input.SkipCount).Take(input.MaxResultCount);     // Applying Paging

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<WorkerFileDto>(
                totalCount,
                ObjectMapper.Map<List<WorkerFileDto>>(entities)
            );
        }
 
        public List<ComboboxItemDto> GetWorkerComboItems(int depotId)        
        {
            var lst = _workerCache.GetList().FindAll(x => x.DepotId == depotId);
            lst.Sort( (a, b) => a.Cn.CompareTo(b.Cn) );

            var list = new List<ComboboxItemDto>();
            foreach (var t in lst)
                list.Add(new ComboboxItemDto { Value = t.Id.ToString(), DisplayText = string.Format("{0} {1}", t.Cn, t.Name) });

            return list;
        }
    }
}