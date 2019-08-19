using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Clc.Clients;

namespace Clc.BoxRecords.Dto
{
    [AutoMapFrom(typeof(Box))]
    public class BoxRecordDto : EntityDto
    {
        public string Cn { get; set; }

        /// <summary>
        /// 物品名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Rfid 
        /// </summary>
        public string Rfid { get; set; }

        public int? BoxRecordId { get; set; }
        public string BoxRecordInfo { get; set; }
        
    }
}