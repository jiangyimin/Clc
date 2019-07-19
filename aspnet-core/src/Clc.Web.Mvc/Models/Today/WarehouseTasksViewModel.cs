using System.Collections.Generic;

namespace Clc.Web.Models.Today
{
    public class WarehouseTasksViewModel
    {
        public string Today { get; set; }
        public int DepotId { get; set; }

        public List<string> WarehouseNames { get; set; }
    }
}