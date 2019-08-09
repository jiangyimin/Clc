using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Clc.Types;

namespace Clc.Types.Dto
{
    [AutoMap(typeof(ArticleType))]
    public class ArticleTypeDto : EntityDto
    {
        [Required]
        [StringLength(ArticleType.MaxNameLength)]
        public string Cn { get; set; }

        [Required]
        [StringLength(ArticleType.MaxNameLength)]
        public string Name { get; set; }       

        /// <summary>
        /// 绑定方式（人，车，线路，机器，网点等）
        /// </summary>
        [StringLength(ArticleType.MaxBindStyleLength)]
        public string BindStyle { get; set; }
    }
}