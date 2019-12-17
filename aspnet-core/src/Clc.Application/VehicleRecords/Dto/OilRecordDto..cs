using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Clc.Runtime;

namespace Clc.VehicleRecords.Dto
{
    [AutoMapFrom(typeof(OilRecord))]
    public class OilRecordDto : EntityDto
    {
        // 创建时间
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 人员（编号 姓名）
        /// </summary>
        public int CreateWorkerId { get; set; }
        public string CreateWorkerName { get; set; }

        // 车辆
        public int VehicleId { get; set; }
        public string VehicleCn { get; set; }
        public string VehicleLicense { get; set; }

        public int GasStationId { get; set; }
        public string GasStationName { get; set; }

        public int OilTypeId { get; set; }
        public string OilTypeName { get; set; }

        public double Quantity { get; set; }
        public double Price { get; set; }

        public double Mileage { get; set; }
        
        
        public string Remark { get; set; }


        // 确认时间
        public DateTime? ConfirmTime { get; set; }

        public int? ProcessWorkerId { get; set; }
        public string ProcessWorkerName { get; set; }

    }
}