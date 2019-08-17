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
        /// 父
        /// </summary>
        [Required]
        public int RouteId { get; set; }

        [Required]
        public int WorkerId { get; set; }
        public string WorkerCn { get; set; }
        public string WorkerName { get; set; }

        [Required]
        public int WorkRoleId { get; set; }
        public string WorkRoleName { get; set; }
        public string WorkRoleArticleTypeList { get; set; }

        public string ArticleList { get; set; }
        
        // only for mds.js 
        public string Postfix { get; } = "Worker";
    }
}

