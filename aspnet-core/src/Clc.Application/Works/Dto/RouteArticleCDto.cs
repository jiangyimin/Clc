using System;
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
        public int ArticleId { get; set;}
        public string DisplayText { get; set; }

        public int RecordId { get; set; }
        public bool IsReturn { get; set; }
        
        public RouteArticleCDto(Article a, int recordId = 0, bool isReturn=false)
        {
            ArticleId = a.Id;
            DisplayText = a.Cn + " " + a.Name;

            RecordId = recordId;
            IsReturn = isReturn;
        }
    }
}

