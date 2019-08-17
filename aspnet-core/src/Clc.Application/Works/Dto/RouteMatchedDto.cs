using System;
using Clc.Fields;

namespace Clc.Works.Dto
{
    public class RouteMatchedDto
    {

        // about Workers
        public int Id { get; set; }
        public string RouteName { get; set; }
        public string VehicleCn { get; set; }
        public string VehicleLicense { get; set; }

        public RouteMatchedDto(RouteCDto r)
        {
            Id = r.Id;
            RouteName = r.RouteName;
            VehicleCn = r.VehicleCn; 
            VehicleLicense = r.VehicleLicense;
        }
    }
}
