using System.Collections.Generic;

namespace Clc.Web.Models.Weixin
{
    public class OilViewModel
    {
        public int WorkerId { get; set; }
        public int VehicleId { get; set; }
        public List<ComboItemModel> Vehicles {get; set; }
        public int GasStationId { get; set; }
        public List<ComboItemModel> GasStations {get; set; }

        public int OilTypeId { get; set; }
        public List<ComboItemModel> OilTypes { get; set; }

        public double Quantity { get; set; }

        public double Price { get; set; }
        public double Mileage { get; set; }

        public string Remark { get; set; }

        public OilViewModel()
        {
            Vehicles = new List<ComboItemModel>();
            GasStations = new List<ComboItemModel>();
            OilTypes = new List<ComboItemModel>();
        }

    }
}