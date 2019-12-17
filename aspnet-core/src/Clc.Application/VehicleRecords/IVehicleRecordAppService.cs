using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Clc.VehicleRecords.Dto;

namespace Clc.VehicleRecords
{
    public interface IVehicleRecordAppService : IApplicationService
    {
        Task<PagedResultDto<OilRecordDto>> GetOilRecordsAsync(PagedAndSortedResultRequestDto requestDto);
        Task<PagedResultDto<VehicleMTRecordDto>> GetVehicleMTRecordsAsync(PagedAndSortedResultRequestDto requestDto);

        Task<VehicleReportDto> GetReportData();

        Task InsertOilRecord(int workerId, int vehicleId, int gasStationId, int oilTypeId, double quantity, double price, double mileage, string remark);
        Task InsertVehicleMTRecord(int workerId, int vehicleId, int vehicleMTTypeId, DateTime mtDate, string content, double price, string remark);

        Task ConfirmOilRecord(int id);
        Task ConfirmVehicleMTRecord(int id);

        Task DeleteOilRecord(int id);
        Task DeleteVehicleMTRecord(int id);
    }
}
