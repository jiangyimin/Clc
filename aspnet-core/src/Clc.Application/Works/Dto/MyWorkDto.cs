using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Clc.Fields;
using Clc.Runtime;

namespace Clc.Works.Dto
{
    /// <summary>
    /// MyWorkModel
    /// </summary>
    public class MyWorkDto
    {
        // Me
        public string WorkerCn { get; set; }        
        
        public int AffairId { get; set; }

        public string Status { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public string Workers { get; set; }

    }
}

