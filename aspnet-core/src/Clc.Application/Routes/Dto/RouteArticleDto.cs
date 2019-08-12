using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Clc.Routes.Dto
{
    /// <summary>
    /// RouteArticleDto
    /// </summary>
    [AutoMap(typeof(RouteArticle))]
    public class RouteArticleDto : EntityDto
    {
        [Required]
        public int RouteId { get; set; }


        /// <summary>
        /// RouteTask
        /// </summary>
        public int RouteWorkerId { get; set; }

        [Required]
        public int ArticleRecordId { get; set; }

        // only for mds.js 
        public string Postfix { get; } = "Article";
    }
}

