using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Clc.Fields;

namespace Clc.Fields.Dto
{
    [AutoMap(typeof(Article))]
    public class ArticleDto : EntityDto
    {
        [Required]
        public int DepotId { get; set; }

        [Required]        
        [StringLength(Article.MaxCnLength)]
        public string Cn { get; set; }

        [Required]
        [StringLength(Article.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        public int ArticleTypeId { get; set; }
        
        [StringLength(Article.MaxRfidLength)]
        public string Rfid { get; set; }

        [StringLength(Article.MaxBindInfoLength)]
        public string BindInfo { get; set; }

    }
}

