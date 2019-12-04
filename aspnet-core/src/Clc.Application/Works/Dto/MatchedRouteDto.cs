using Clc.Routes;

namespace Clc.Works.Dto
{
    public class MatchedRouteDto
    {

        // about Workers
        public int Id { get; set; }
        public string RouteName { get; set; }
        public string VehicleCn { get; set; }
        public string VehicleLicense { get; set; }

        public MatchedRouteDto(string routeInfo)
        {
            var ss = routeInfo.Split();
            Id = int.Parse(ss[0]);
            RouteName = ss[1];
        }
        
        public MatchedRouteDto(RouteCacheItem r)
        {
            Id = r.Id;
            RouteName = r.RouteName;
            if (!r.AltVehicleId.HasValue) 
            {
                VehicleCn = r.VehicleCn; 
                VehicleLicense = r.VehicleLicense;
            }
            else
            {
                VehicleCn = r.AltVehicleCn; 
                VehicleLicense = r.AltVehicleLicense;
            }
        }
    }
}
