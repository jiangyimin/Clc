using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Clc.Runtime;

namespace Clc.DoorRecords.Dto
{
    [AutoMapFrom(typeof(EmergDoorRecord))]
    public class EmergDoorRecordDto : EntityDto
    {
        public string IssueContent { get; set; }

        public string WorkplaceName { get; set; }
    }
}