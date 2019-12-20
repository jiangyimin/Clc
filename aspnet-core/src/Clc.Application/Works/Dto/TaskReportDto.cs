using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Clc.Routes;

namespace Clc.Works.Dto
{
    public class CountPair
    {
        public int Count1 { get; set; }
        public int Count2 { get; set; }

        public CountPair(int c1, int c2) 
        {
            Count1 = c1;
            Count2 = c2;
        }
    }

    [AutoMap(typeof(RouteTask))]
    public class TemporaryTaskDto : EntityDto
    {
        public DateTime RouteCarryoutDate { get; set; }
        public string RouteDepotName { get; set; }
        public string RouteRouteName { get; set; }

        public string OutletCustomerName { get; set; }
        public int OutletId { get; set; }
        public string OutletCn { get; set; }
        public string OutletName { get; set; }

        public string TaskTypeName { get; set; }
        public int TaskTypeBasicPrice { get; set; }

        public string Remark { get; set; }

        public string ArriveTime { get; set; }
        public DateTime? IdentifyTime { get; set; }

        public float? Price { get; set; }
    }

    /// <summary>
    /// TaskReportDto
    /// </summary>
    public class TaskReportDto
    {
        public CountPair Route { get; set; }
        public CountPair Affair { get; set; }
        public CountPair Task { get; set; }

        public List<TemporaryTaskDto> Tasks { get; set; }
    }
}

