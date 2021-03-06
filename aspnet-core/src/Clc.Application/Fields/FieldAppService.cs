using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Linq;
using Abp.Linq.Extensions;
using Clc.Configuration;
using Clc.Fields.Dto;
using Clc.Runtime.Cache;
using Clc.Works;

namespace Clc.Fields
{
    [AbpAuthorize]
    public class FieldAppService : ClcAppServiceBase, IFieldAppService
    {
        public WorkManager WorkManager { get; set; }
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        private readonly IDepotCache _depotCache;
        private readonly IWorkplaceCache _workplaceCache;
        private readonly IWorkerCache _workerCache;
        private readonly IVehicleCache _vehicleCache;
        private readonly IPostCache _postCache;
        private readonly IWorkRoleCache _workRoleCache;
        private readonly IGasStationCache _gasStationCache;

        private readonly IRepository<Worker> _workerRepository;
        private readonly IRepository<WorkerFile> _workerFileRepository;
        private readonly IRepository<Asset> _assetRepository;
        public FieldAppService(IDepotCache depotCache,
            IWorkplaceCache workplaceCache,
            IWorkerCache workerCache,
            IVehicleCache vehicleCache,
            IPostCache postCache,
            IWorkRoleCache workRoleCache,
            IGasStationCache gasStationCache,
            IRepository<Worker> workerRepository, 
            IRepository<WorkerFile> workerFileRepository,
            IRepository<Asset> assetRepository)
        {
            _depotCache = depotCache;
            _workplaceCache = workplaceCache;
            _workerCache = workerCache;
            _vehicleCache = vehicleCache;
            _postCache = postCache;
            _workRoleCache = workRoleCache;
            _gasStationCache = gasStationCache;
            _workerRepository = workerRepository;
            _workerFileRepository = workerFileRepository;
            _assetRepository = assetRepository;
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
                case "GasStation":
                    foreach (GasStation t in _gasStationCache.GetList())
                        lst.Add(new ComboboxItemDto { Value = t.Id.ToString(), DisplayText = t.Name });
                    break;
                default:
                    break;
            }
            return lst;
        }

        public List<Workplace> GetWorkplaceItems(bool justVault)
        {
            int depotId = _workerCache[GetCurrentUserWorkerIdAsync().Result].DepotId;
            var lst = _workplaceCache.GetList();
            if (justVault)
                return lst.FindAll(x => x.DepotId == depotId && x.Name.Contains("金库"));
            else
                return lst.FindAll(x => x.DepotId == depotId);
        }

        public List<ComboboxItemDto> GetWorkerItemsByWorkRole(int workRoleId)        
        {
            var list = new List<ComboboxItemDto>();

            int depotId = _workerCache[GetCurrentUserWorkerIdAsync().Result].DepotId;
            var lst = _workerCache.GetList().FindAll(x => x.DepotId == depotId && x.IsActive == true);
            lst.Sort( (a, b) => a.Cn.CompareTo(b.Cn) );

            var role = _workRoleCache[workRoleId];
            foreach (var w in lst)
            {
                var post = _postCache[w.PostId];

                if (string.IsNullOrEmpty(w.WorkRoleNames))
                {
                    if (!string.IsNullOrEmpty(post.DefaultWorkRoleName) && post.DefaultWorkRoleName == role.Name) 
                        list.Add(new ComboboxItemDto { Value = w.Id.ToString(), DisplayText = w.CnNamePost });
                }
                else
                {
                    var roles = w.WorkRoleNames.Split(',', '|');
                    if (roles.Contains(role.Name))
                        list.Add(new ComboboxItemDto { Value = w.Id.ToString(), DisplayText = w.CnNamePost });
                }
            }
            return list;
        }

        public List<WorkerCacheItem> GetWorkerCacheItems(bool all)        
        {
            if (all)
            {
                return ObjectMapper.Map<List<WorkerCacheItem>>(_workerCache.GetList().FindAll(x => x.IsActive == true));
            }
            else
            {
                int depotId = _workerCache[GetCurrentUserWorkerIdAsync().Result].DepotId;
                var lst = _workerCache.GetList().FindAll(x => x.DepotId == depotId);
                lst.Sort( (a, b) => a.Cn.CompareTo(b.Cn) );
                return lst;
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
        public async Task<PagedResultDto<WorkerFingerDto>> GetWorkerFingersAsync(PagedAndSortedResultRequestDto input)
        {
            int depotId = _workerCache[GetCurrentUserWorkerIdAsync().Result].DepotId;
            var query = _workerRepository.GetAll().Where(x => x.DepotId == depotId);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            if (!string.IsNullOrWhiteSpace(input.Sorting))
                query = query.OrderBy(input.Sorting);                               // Applying Sorting
            query = query.Skip(input.SkipCount).Take(input.MaxResultCount);     // Applying Paging

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<WorkerFingerDto>(
                totalCount,
                ObjectMapper.Map<List<WorkerFingerDto>>(entities)
            );
        }

        public async Task<WorkerFingerDto> UpdateWorkerFingerAsync(WorkerFingerDto input)
        {
            var entity = await _workerRepository.GetAsync(input.Id);
            ObjectMapper.Map<WorkerFingerDto, Worker>(input, entity);

            await _workerRepository.UpdateAsync(entity);
            CurrentUnitOfWork.SaveChanges();
            return ObjectMapper.Map<WorkerFingerDto>(entity);
        }

        public async Task<PagedResultDto<AssetDto>> SearchAssetPagedResult(PagedAssetResultRequestDto input)
        {
            var query = _assetRepository.GetAllIncluding(x => x.Depot)
                .WhereIf(input.DepotId.HasValue, x => x.DepotId == input.DepotId.Value)
                .WhereIf(input.Category != null, x => x.Category == input.Category);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            if (!string.IsNullOrWhiteSpace(input.Sorting))
                query = query.OrderBy(input.Sorting);                               // Applying Sorting
            query = query.Skip(input.SkipCount).Take(input.MaxResultCount);     // Applying Paging

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<AssetDto>(
                totalCount,
                ObjectMapper.Map<List<AssetDto>>(entities)
            );
        }
        
        public async Task<PagedResultDto<WorkerFileDto>> SearchFilePagedResult(PagedFileResultRequestDto input)
        {
            var query = _workerFileRepository.GetAllIncluding(x => x.Worker, x => x.Worker.Depot, x => x.Worker.Post)
                .WhereIf(input.DepotId.HasValue, x => x.Worker.DepotId == input.DepotId.Value)
                .WhereIf(input.PostId.HasValue, x => x.Worker.DepotId == input.PostId.Value)
                .WhereIf(input.Status != null, x => x.Status == input.Status);

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

        public bool AllowEditWorkerFinger()
        {
            int workerId = GetCurrentUserWorkerIdAsync().Result;
            var depot = WorkManager.GetDepot(WorkManager.GetWorker(workerId).DepotId);

            var depots = SettingManager.GetSettingValue(AppSettingNames.Rule.EditWorkerDepots);
            if (!string.IsNullOrEmpty(depots) && depots.Split('|', ',').Contains(depot.Name)) 
                return true;
            return false;
        }

    }
}