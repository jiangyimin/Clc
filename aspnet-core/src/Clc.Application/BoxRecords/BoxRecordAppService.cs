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
using Clc.Fields;
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
        private readonly IRepository<RouteInBox> _routeInBoxRepository;
        private readonly IRepository<RouteOutBox> _routeOutBoxRepository;

        public BoxRecordAppService(IRepository<BoxRecord> recordRepository, 
            IRepository<Box> boxRepository,
            IRepository<RouteInBox> routeInBoxRepository,
            IRepository<RouteOutBox> routeOutBoxRepository)    
        {
            _recordRepository = recordRepository;
            _boxRepository = boxRepository;
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

        public int In(int routeId, List<RouteBoxCDto> boxes, string workers)
        {
            foreach (var box in boxes)
            {               
                BoxRecord record = new BoxRecord() {
                    BoxId = box.BoxId,
                    InTime = DateTime.Now,
                    InWorkers = workers
                };

                int recordId = _recordRepository.InsertAndGetId(record);
                Box b = _boxRepository.Get(box.BoxId);
                b.BoxRecordId = recordId;

                RouteInBox ri = new RouteInBox() {
                    RouteId = routeId,
                    RouteTaskId = box.TaskId,
                    BoxRecordId = recordId
                };
                _routeInBoxRepository.Insert(ri);
            }
            return boxes.Count();
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

        public int Out(int routeId, List<RouteBoxCDto> boxes, string workers)
        {
            foreach (var box in boxes)
            {               
                var record = _recordRepository.Get(box.RecordId);   
                record.OutTime = DateTime.Now;
                record.OutWorkers = workers;

                RouteOutBox ro = new RouteOutBox() {
                    RouteId = routeId,
                    RouteTaskId = box.TaskId,
                    BoxRecordId = box.RecordId
                };
                _routeOutBoxRepository.Insert(ro);
            }  
            return boxes.Count; 
        }

        public async Task<List<BoxRecordSearchDto>> SearchByDay(DateTime theDay)
        {
            var query = _recordRepository.GetAllIncluding(x => x.Box)
                .Where(x => x.InTime.Date == theDay);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            return  entities.Select(MapToSearchDto).ToList(); 
        }

        public async Task<List<BoxRecordSearchDto>> SearchByBoxId(int boxId, DateTime begin, DateTime end)
        {
            var query = _recordRepository.GetAllIncluding(x => x.Box)
                .Where(x => x.BoxId == boxId && x.InTime.Date >= begin && x.InTime.Date <= end);
            
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            return  entities.Select(MapToSearchDto).ToList(); 
        }

        public async Task<List<BoxReportDto>> GetReportData()
        {
            var query = _boxRepository.GetAllIncluding(x => x.Outlet, x => x.BoxRecord)
                .Where(x => x.BoxRecord != null && x.BoxRecord.InTime.Date == DateTime.Now.Date)
                .Select( x => new BoxReportDto {
                    OutletName = x.Outlet.Name, 
                    ToUser = x.Outlet.Weixins,
                    BoxName = x.Name,
                    InTime = x.BoxRecord.InTime.ToString("yyyy/MM/dd HH:mm")
                });
            return await AsyncQueryableExecuter.ToListAsync(query);            
        }

        #region util

        private BoxRecordDto MapToBoxRecordDto(Box entity)
        {
            var dto = ObjectMapper.Map<BoxRecordDto>(entity);

            var record = entity.BoxRecord;
            if (record == null) return dto;

        
            string l = $"入库时间：{record.InTime.ToString("yyyy-MM-dd HH:mm")} ";
            string r = record.OutTime.HasValue ? $"【出库时间：{record.OutTime.Value.ToString("yyyy-MM-dd HH:mm")}】" : "【未出库】";
            dto.BoxRecordInfo = l + r;
            return dto;
        }

        private BoxRecordSearchDto MapToSearchDto(BoxRecord record)
        {
            var dto = new BoxRecordSearchDto();
            dto.Box = record.Box.Name;
            // dto.Worker = worker.Cn + ' ' + worker.Name;
            dto.InTime = record.InTime.ToString("yyyy-MM-dd HH:mm:ss");
            dto.OutTime = record.OutTime.HasValue ? record.OutTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : null;
            dto.InWorkers = record.InWorkers;
            dto.OutWorkers = record.OutWorkers;
            return dto;
        }
        #endregion
    }
}