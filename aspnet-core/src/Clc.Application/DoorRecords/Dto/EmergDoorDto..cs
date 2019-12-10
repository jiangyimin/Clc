using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Clc.Runtime;

namespace Clc.DoorRecords.Dto
{
    [AutoMapFrom(typeof(EmergDoorRecord))]
    public class EmergDoorDto : EntityDto
    {
        public DateTime CreateTime { get; set; }

        public string WorkplaceDepotName { get; set; }
        public string WorkplaceName { get; set; }
        public string WorkplaceDoorIp { get; set; }
        public string IssueContent { get; set; }

        /// <summary>
        /// 审批信息：审批人
        /// </summary>
        public string ApproverName { get; set; }
        public DateTime? ApprovalTime { get; set; }

        //public string EmergDoorPassword { get; set; }
        //public string WorkplaceEmergPassword { get; set; }
    }
}