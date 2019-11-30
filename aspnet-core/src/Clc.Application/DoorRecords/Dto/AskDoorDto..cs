using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Clc.Runtime;

namespace Clc.DoorRecords.Dto
{
    [AutoMapFrom(typeof(AskDoorRecord))]
    public class AskDoorDto : EntityDto
    {
        public string WorkplaceDoorIp { get; set; }
        public DateTime AskTime { get; set; }

        public string WorkplaceDepotName { get; set; }
        /// <summary>
        /// 对应的门
        /// </summary>
        public string WorkplaceName { get; set; }
        public string WorkplaceCameraIp { get; set; }

        public int AskAffairId { get; set; }
        public string AskAffairContent { get; set; }
        public string AskStyle { get; set; }

        public string AskWorkers { get; set; }
    }
}