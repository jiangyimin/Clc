using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq;
using Clc.BoxRecords.Dto;
using Clc.Authorization;
using Clc.Routes;
using Clc.Runtime;
using Clc.Works;
using Clc.Clients;
using Clc.Works.Dto;

namespace Clc.BoxRecords
{
    [AbpAuthorize(PermissionNames.Pages_Box, PermissionNames.Pages_Arrange)]
    public class BoxRecordAppService : ClcAppServiceBase, IBoxRecordAppService
    {
        public WorkManager WorkManager { get; set; }
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IRepository<BoxRecord> _recordRepository;
        private readonly IRepository<Box> _boxRepository;
        private readonly IRepository<RouteTask> _routeTaskRepository;
        private readonly IRepository<RouteInBox> _routeInBoxRepository;
        private readonly IRepository<RouteOutBox> _routeOutBoxRepository;

        public BoxRecordAppService(IRepository<BoxRecord> recordRepository, 
            IRepository<Box> boxRepository,
            IRepository<RouteTask> routeTaskRepository,
            IRepository<RouteInBox> routeInBoxRepository,
            IRepository<RouteOutBox> routeOutBoxRepository)    
        {
            _recordRepository = recordRepository;
            _boxRepository = boxRepository;
            _routeTaskRepository = routeTaskRepository;
            _routeInBoxRepository = routeInBoxRepository;
            _routeOutBoxRepository = routeOutBoxRepository;
        }

        public async Task<PagedResultDto<BoxRecordDto>> GetBoxesAsync(PagedAndSortedResultRequestDto input)
        {         
            var query = _boxRepository.GetAllIncluding(x => x.BoxRecord);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            if (!string.IsNullOrWhiteSpace(input.Sorting))
                query = query.OrderBy(input.Sorting);                               // Applying Sorting
            query = query.Skip(input.SkipCount).Take(input.MaxResultCount);         // Applying Paging

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<BoxRecordDto>(
                totalCount,
                entities.Select(MapToBoxRecordDto).ToList()
            );
        }

        public string InBox(int taskId, string boxCn, string workers)
        {
            var task = _routeTaskRepository.Get(taskId);
            var boxes = _routeInBoxRepository.GetAllIncluding(x => x.BoxRecord, x => x.BoxRecord.Box)
                .Where(x => x.RouteTaskId == taskId).ToList();

            if (boxes != null & boxes.Count > 0)
            {
                foreach (var bo in boxes)
                    if (bo.BoxRecord.Box.Cn == boxCn && bo.BoxRecord.InTime.HasValue) return "此尾箱已入库";               
            }

            var box = WorkManager.GetBoxByCn(boxCn);
            if (box.Id == 0) return "无此尾箱";

            var record = new BoxRecord() {
                BoxId = box.Id,
                InTime = DateTime.Now,
                InWorkers = workers
            };
            int recordId = _recordRepository.InsertAndGetId(record);

            Box b = _boxRepository.Get(box.Id);
            b.BoxRecordId = recordId;

            RouteInBox ri = new RouteInBox() {
                RouteId = task.RouteId,
                RouteTaskId = task.Id,
                BoxRecordId = recordId
            };
            
            _routeInBoxRepository.Insert(ri);
            
            return null;
        }

        public string GetBoxStatus(int boxId) 
        {
            var box = _boxRepository.Get(boxId);

            if (box.BoxRecordId.HasValue)
            {
                var record = _recordRepository.Get(box.BoxRecordId.Value);
                if (!record.OutTime.HasValue)
                {
                    return "此尾箱还在库中";
                }
            }
            return null;
        }

        public string OutBox(int taskId, string boxCn, string workers)
        {
            var task = _routeTaskRepository.Get(taskId);
            var boxes = _routeInBoxRepository.GetAllIncluding(x => x.BoxRecord, x => x.BoxRecord.Box)
                .Where(x => x.RouteTaskId == taskId).ToList();

            if (boxes != null & boxes.Count > 0)
            {
                foreach (var b in boxes)
                    if (b.BoxRecord.Box.Cn == boxCn && b.BoxRecord.OutTime.HasValue) 
                        return "此尾箱已出库";
            }

            var box = WorkManager.GetBoxByCn(boxCn);
            if (box.Id == 0) return "无此尾箱";

            if (!box.BoxRecordId.HasValue) return "此箱需要先入库";
            var record = _recordRepository.Get(box.BoxRecordId.Value);
            record.OutTime = DateTime.Now;
            record.OutWorkers = workers;
        
            RouteOutBox ro = new RouteOutBox() {
                RouteId = task.RouteId,
                RouteTaskId = task.Id,
                BoxRecordId = box.BoxRecordId.Value
            };
            
            _routeOutBoxRepository.Insert(ro);
            
            return null;
        }

        public async Task<List<BoxRecordSearchDto>> SearchByDay(DateTime theDay)
        {
            var query = _recordRepository.GetAllIncluding(x => x.Box)
                .Where(x => x.InTime.Value.Date == theDay);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            return  entities.Select(MapToSearchDto).ToList(); 
        }

        public async Task<List<BoxRecordSearchDto>> SearchByBoxId(int boxId, DateTime begin, DateTime end)
        {
            var query = _recordRepository.GetAllIncluding(x => x.Box)
                .Where(x => x.BoxId == boxId && x.InTime.Value.Date >= begin && x.InTime.Value.Date <= end);
            
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            return  entities.Select(MapToSearchDto).ToList(); 
        }

        public async Task<List<BoxReportDto>> GetReportData()
        {
            var query = _boxRepository.GetAllIncluding(x => x.Outlet, x => x.BoxRecord)
                .Where(x => x.BoxRecord != null && x.BoxRecord.InTime.Value.Date == DateTime.Now.Date)
                .Select( x => new BoxReportDto {
                    OutletName = x.Outlet.Name, 
                    ToUser = x.Outlet.Weixins,
                    BoxName = x.Name,
                    InTime = x.BoxRecord.InTime.Value.ToString("yyyy/MM/dd HH:mm")
                });
            return await AsyncQueryableExecuter.ToListAsync(query);            
        }

        #region util

        private BoxRecordDto MapToBoxRecordDto(Box entity)
        {
            var dto = ObjectMapper.Map<BoxRecordDto>(entity);

            var record = entity.BoxRecord;
            if (record == null) return dto;

        
            string l = $"入库时间：{record.InTime.Value.ToString("yyyy-MM-dd HH:mm")} ";
            string r = record.OutTime.HasValue ? $"【出库时间：{record.OutTime.Value.ToString("yyyy-MM-dd HH:mm")}】" : "【未出库】";
            dto.BoxRecordInfo = l + r;
            return dto;
        }

        private BoxRecordSearchDto MapToSearchDto(BoxRecord record)
        {
            var dto = new BoxRecordSearchDto();
            dto.Box = record.Box.Name;
            // dto.Worker = worker.Cn + ' ' + worker.Name;
            dto.InTime = record.InTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
            dto.OutTime = record.OutTime.HasValue ? record.OutTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : null;
            dto.InWorkers = record.InWorkers;
            dto.OutWorkers = record.OutWorkers;
            return dto;
        }
        #endregion
    }
}