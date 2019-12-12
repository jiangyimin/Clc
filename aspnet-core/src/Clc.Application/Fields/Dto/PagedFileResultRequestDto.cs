using Abp.Application.Services.Dto;
using System;

namespace Clc.Fields.Dto
{
    //custom PagedResultRequestDto
    public class PagedFileResultRequestDto : PagedAndSortedResultRequestDto
    {
        public int? DepotId { get; set; }
        public int? PostId { get; set; }
        public string Status { get; set; }

    }
}
