using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq;
using Clc.VehicleRecords.Dto;
using Clc.Authorization;
using Clc.Runtime;
using Clc.Works;

namespace Clc.VehicleRecords
{
    [AbpAuthorize(PermissionNames.Pages_Arrange)]
    public class VehicleRecordAppService : ClcAppServiceBase, IVehicleRecordAppService
    {
        public WorkManager WorkManager { get; set; }
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        private readonly IRepository<OilRecord> _oilRecordRepository;
        private readonly IRepository<VehicleMTRecord> _vehicleMTRecordRepository;

        public VehicleRecordAppService(
            IRepository<OilRecord> oilRecordRepository, 
            IRepository<VehicleMTRecord> vehicleMTRecordRepository)
        {
            _oilRecordRepository = oilRecordRepository;
            _vehicleMTRecordRepository = vehicleMTRecordRepository;
        }

        public async Task<PagedResultDto<OilRecordDto>> GetOilRecordsAsync(PagedAndSortedResultRequestDto input)

        {
            int depotId = WorkManager.GetWorkerDepotId(await GetCurrentUserWorkerIdAsync());
            
            var query = _oilRecordRepository.GetAllIncluding(x => x.CreateWorker, x => x.Vehicle, x => x.GasStation, x => x.OilType, x => x.ProcessWorker)
                .Where(x => x.Vehicle.DepotId == depotId);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            if (!string.IsNullOrWhiteSpace(input.Sorting))
                query = query.OrderBy(input.Sorting);                               // Applying Sorting
            query = query.Skip(input.SkipCount).Take(input.MaxResultCount);         // Applying Paging

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<OilRecordDto>(
                totalCount,
                ObjectMapper.Map<List<OilRecordDto>>(entities)
            );
        }

        public async Task<VehicleReportDto> GetReportData() 
        {
            int depotId = WorkManager.GetWorkerDepotId(await GetCurrentUserWorkerIdAsync());
            var query = _oilRecordRepository.GetAllIncluding(x => x.Vehicle)
                .Where(x => x.CreateTime.Date == DateTime.Now.Date && x.Vehicle.DepotId == depotId)
                .GroupBy( x => x.Vehicle.DepotId )
                .Select( p => new VehicleReportDto {
                    OilCount = p.Count(),
                    OilQuantity = p.Sum(x => x.Quantity),
                    OilPrice = p.Sum(x => x.Price),
                });               
            var list = await AsyncQueryableExecuter.ToListAsync(query);
            var ret = new VehicleReportDto();
            if (list.Count > 0)
                ret = list[0];

            var queryMT = _vehicleMTRecordRepository.GetAllIncluding(x => x.Vehicle)
                .Where(x => x.CreateTime.Date == DateTime.Now.Date && x.Vehicle.DepotId == depotId)
                .GroupBy( x => x.Vehicle.DepotId )
                .Select( p => new {
                    MTCount = p.Count(),
                    MTPrice = p.Sum(x => x.Price),
                });               
            var listMT = await AsyncQueryableExecuter.ToListAsync(queryMT);
            if (listMT.Count > 0) {
                ret.MTCount = listMT[0].MTCount;
                ret.MTPrice = listMT[0].MTPrice;
            }
            return ret;
        }

        public async Task<PagedResultDto<VehicleMTRecordDto>> GetVehicleMTRecordsAsync(PagedAndSortedResultRequestDto input)
        {
            int depotId = WorkManager.GetWorkerDepotId(await GetCurrentUserWorkerIdAsync());
            
            var query = _vehicleMTRecordRepository.GetAllIncluding(x => x.CreateWorker, x => x.Vehicle, x => x.VehicleMTType, x => x.ProcessWorker)
                .Where(x => x.Vehicle.DepotId == depotId);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            if (!string.IsNullOrWhiteSpace(input.Sorting))
                query = query.OrderBy(input.Sorting);                               // Applying Sorting
            query = query.Skip(input.SkipCount).Take(input.MaxResultCount);         // Applying Paging

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<VehicleMTRecordDto>(
                totalCount,
                ObjectMapper.Map<List<VehicleMTRecordDto>>(entities)
            );
        }

        public async Task ConfirmOilRecord(int id) 
        {
            var worker = WorkManager.GetWorker(await GetCurrentUserWorkerIdAsync());
            var entity = await _oilRecordRepository.GetAsync(id);
            entity.ConfirmTime = DateTime.Now;
            entity.ProcessWorkerId = worker.Id;
            await _oilRecordRepository.UpdateAsync(entity);           
        }

        public async Task ConfirmVehicleMTRecord(int id) 
        {
            var worker = WorkManager.GetWorker(await GetCurrentUserWorkerIdAsync());
            var entity = await _vehicleMTRecordRepository.GetAsync(id);
            entity.ConfirmTime = DateTime.Now;
            entity.ProcessWorkerId = worker.Id;
            await _vehicleMTRecordRepository.UpdateAsync(entity);                    
        }

        public async Task DeleteOilRecord(int id) 
        {
            var entity = await _oilRecordRepository.GetAsync(id);
            _oilRecordRepository.Delete(entity);           
        }

        public async Task DeleteVehicleMTRecord(int id) 
        {
            var entity = await _vehicleMTRecordRepository.GetAsync(id);
            _vehicleMTRecordRepository.Delete(entity);           
        }

        [AbpAllowAnonymous]
        public async Task InsertOilRecord(int workerId, int vehicleId, int gasStationId, int oilTypeId, double quantity, double price, double mileage, string remark)
        {
            var entity = new OilRecord();
            entity.TenantId = 1;
            entity.CreateTime = DateTime.Now;
            entity.CreateWorkerId = workerId;
            entity.VehicleId = vehicleId;
            entity.GasStationId = gasStationId;
            entity.OilTypeId = oilTypeId;
            entity.Quantity = quantity;
            entity.Price = price;
            entity.Mileage = mileage;
            entity.Remark = remark;

            await _oilRecordRepository.InsertAsync(entity);
            CurrentUnitOfWork.SaveChanges();

        }
        [AbpAllowAnonymous]
        public async Task InsertVehicleMTRecord(int workerId, int vehicleId, int vehicleMTTypeId, DateTime mtDate, string content, double price, string remark)
        {
            var entity = new VehicleMTRecord();
            entity.TenantId = 1;
            entity.CreateTime = DateTime.Now;
            entity.CreateWorkerId = workerId;
            entity.VehicleId = vehicleId;
            entity.VehicleMTTypeId = vehicleMTTypeId;
            entity.MTDate = mtDate;
            entity.Content = content;
            entity.Price = price;
            entity.Remark = remark;

            await _vehicleMTRecordRepository.InsertAsync(entity);
            CurrentUnitOfWork.SaveChanges();
        }

    }
}