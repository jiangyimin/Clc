using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Clc.Fields;
using Clc.Routes;

namespace Clc.Works.Dto
{
    /// <summary>
    /// RouteArticleCDto
    /// </summary>
    public class RouteArticleCDto
    {
        public int RecordId { get; set; }

        public int ArticleId { get; set;}
        public string DisplayText { get; set; }

        public RouteArticleCDto(Article a)
        {
            ArticleId = a.Id;
            DisplayText = a.Cn + " " + a.Name;
        }
    }
}

