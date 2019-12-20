using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Clc.Routes;

namespace Clc.PreRoutes.Dto
{
    /// <summary>
    /// PreVehicleWorkerDto
    /// </summary>
    [AutoMap(typeof(PreVehicleWorker))]
    public class PreVehicleWorkerDto : EntityDto
    {
         /// <summary>
        /// 父
        /// </summary>
        [Required]
        public int VehicleId { get; set; }

        [Required]
        public int WorkerId { get; set; }
        public string WorkerCn { get; set; }
        public string WorkerName { get; set; }

        [Required]
        public int WorkRoleId { get; set; }
        public string WorkRoleName { get; set; }
        public string WorkRoleArticleTypeList { get; set; }
    }
}

