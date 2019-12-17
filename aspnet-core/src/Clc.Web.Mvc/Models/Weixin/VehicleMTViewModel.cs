using System;
using System.Collections.Generic;

namespace Clc.Web.Models.Weixin
{
    public class VehicleMTViewModel
    {
        public int WorkerId { get; set; }
        public int VehicleId { get; set; }
        public List<ComboItemModel> Vehicles {get; set; }

        public int VehicleMTTypeId { get; set; }
        public List<ComboItemModel> VehicleMTTypes {get; set; }

        public DateTime MTDate { get; set; }
        public string Content { get; set; }

        public double Price { get; set; }
        public string Remark { get; set; }

    }
}