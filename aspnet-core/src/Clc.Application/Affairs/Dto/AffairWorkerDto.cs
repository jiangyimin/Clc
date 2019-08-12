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

        [Required]
        public int WorkRoleId { get; set; }
        public string WorkRoleName { get; set; }
        
        // only for mds.js 
        public string Postfix { get; } = "Worker";
    }
}

