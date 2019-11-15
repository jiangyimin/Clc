using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Clc.Runtime;

namespace Clc.DoorRecords.Dto
{
    [AutoMapFrom(typeof(AskDoorRecord))]
    public class AskDoorRecordDto : EntityDto
    {
        public string Cn { get; set; }

        /// <summary>
        /// 物品名
        /// </summary>
        public string Name { get; set; }
        public int AskDoorTypeId { get; set; }
        public string AskDoorTypeName { get; set; }

        /// <summary>
        /// Rfid 
        /// </summary>
        public string Rfid { get; set; }

        /// <summary>
        /// 绑定信息 
        /// </summary>
        public string BindInfo { get; set; }

        public int? AskDoorRecordId { get; set; }
        public string AskDoorRecordInfo { get; set; }
        
    }
}