using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Clc.Runtime;

namespace Clc.Issues.Dto
{
    [AutoMapFrom(typeof(Issue))]
    public class IssueDto : EntityDto
    {
        // 创建时间
        public DateTime CreateTime { get; set; }

        // 大队
        public int DepotId { get; set; }

        public string DepotName { get; set; }
        public string CreateWorkerName { get; set; }
        
        public string Content { get; set; }

        // 目前有三种：1）报告  2）应急开门 3) 值班
        public string ProcessStyle { get; set; }


        // 处理时间
        public DateTime? ProcessTime { get; set; }
        public string ProcessWorkerName { get; set; }

        public string ProcessContent { get; set; }
    }
}