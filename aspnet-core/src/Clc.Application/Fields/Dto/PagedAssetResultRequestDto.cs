using Abp.Application.Services.Dto;
using System;

namespace Clc.Fields.Dto
{
    //custom PagedResultRequestDto
    public class PagedAssetResultRequestDto : PagedAndSortedResultRequestDto
    {
        public int? DepotId { get; set; }
        public string Category { get; set; }

    }
}
