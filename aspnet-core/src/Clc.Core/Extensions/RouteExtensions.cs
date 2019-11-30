using Clc.Routes;

namespace Clc.Extensions
{
    public static class RouteExtensions
    {
        public static int GetFactVehicleId(this Route r)
        {
            return r.AltVehicleId.HasValue ? r.AltVehicleId.Value : r.VehicleId;
        }
        
        public static int GetFactWorkerId(this RouteWorker rw)
        {
            return rw.AltWorkerId.HasValue ? rw.AltWorkerId.Value : rw.WorkerId;
        }
        public static int GetFactWorkerId(this RouteWorkerCacheItem rw)
        {
            return rw.AltWorkerId.HasValue ? rw.AltWorkerId.Value : rw.WorkerId;
        }
    }
}
