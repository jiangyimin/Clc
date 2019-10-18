using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Clc.Runtime;

namespace Clc.BoxRecords.Dto
{
    [AutoMapFrom(typeof(DoorRecord))]
    public class DoorRecordDto : EntityDto
    {
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 物品名
        /// </summary>
        public string WorkplaceName { get; set; }

        public string OpenWorkers { get; set; }

        public string ApplyWorkers { get; set; }
        public int OpenAppairId { get; set; }
        public int? ApplyAffairId { get; set; }
    }
}