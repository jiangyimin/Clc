using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Clc.Fields.Dto
{
    [AutoMap(typeof(ArticleTypeBind))]
    public class ArticleTypeBindDto : EntityDto
    {
        [Required]
        public int DepotId { get; set; }

        [Required]
        public int ArticleTypeId { get; set; }

        /// <summary>
        /// 绑定方式（人，车，线路，机器，网点等）
        /// </summary>
        [StringLength(ArticleTypeBind.MaxBindStyleLength)]
        public string BindStyle { get; set; }
    }
}

