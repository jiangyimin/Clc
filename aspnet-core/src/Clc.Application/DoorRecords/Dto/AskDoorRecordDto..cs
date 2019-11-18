using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Clc.Runtime;

namespace Clc.DoorRecords.Dto
{
    [AutoMapFrom(typeof(AskDoorRecord))]
    public class AskDoorRecordDto : EntityDto
    {
        public DateTime AskTime { get; set; }

        /// <summary>
        /// 对应的门
        /// </summary>
        public string WorkplaceName { get; set; }
        public int AskffairId { get; set; }
        public string AskWorkers { get; set; }

        public int? MonitorAffairId { get; set; }
        public string MonitorWorkers { get; set; }
        public DateTime? ProcessTime { get; set; }
    }
}