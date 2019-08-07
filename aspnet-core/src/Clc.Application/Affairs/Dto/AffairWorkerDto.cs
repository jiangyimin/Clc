using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Clc.Works.Entities;

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

        [Required]
        public int WorkRoleId { get; set; }
    }
}

