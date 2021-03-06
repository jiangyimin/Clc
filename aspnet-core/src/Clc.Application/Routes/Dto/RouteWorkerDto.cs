using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Clc.Routes.Dto
{
    /// <summary>
    /// RouteWorkerDto
    /// </summary>
    [AutoMap(typeof(RouteWorker))]
    public class RouteWorkerDto : EntityDto
    {
         /// <summary>
        /// RouteId
        /// </summary>
        [Required]
        public int RouteId { get; set; }

        [Required]
        public int WorkerId { get; set; }
        public string WorkerCn { get; set; }
        public string WorkerName { get; set; }

        public int? AltWorkerId { get; set; }
        public string AltWorkerCn { get; set; }
        public string AltWorkerName { get; set; }

        [Required]
        public int WorkRoleId { get; set; }
        public string WorkRoleName { get; set; }
        public string WorkRoleArticleTypeList { get; set; }

        public string ArticleList { get; set; }

        public string Signin { get; set; }
        
        // only for mds.js 
        public string Postfix { get; } = "Worker";
    }
}

