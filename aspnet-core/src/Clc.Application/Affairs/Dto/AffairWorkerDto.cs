using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Clc.Affairs;

namespace Clc.Affairs
{
    /// <summary>
    /// AffairWorkerDto
    /// </summary>
    [AutoMap(typeof(AffairWorker))]
    public class AffairWorkerDto : EntityDto
    {
         /// <summary>
        /// 父
        /// </summary>
        [Required]
        public int AffairId { get; set; }

        [Required]
        public int WorkerId { get; set; }
        public string WorkerCn { get; set; }
        public string WorkerName { get; set; }
        public string WorkerRfid { get; set; }

        [Required]
        public int WorkRoleId { get; set; }
        public string WorkRoleName { get; set; }
        public string WorkRoleDuties { get; set; }
        
        public string PhotoString { get; set; }
        
        public bool OnDuty { get; set; }

        public DateTime? CheckinTime { get; set; }
        public DateTime? CheckoutTime { get; set; }

        public DateTime? LastAskDoor { get; set; }

        // only for mds.js 
        public string Postfix { get; } = "Worker";
    }
}

