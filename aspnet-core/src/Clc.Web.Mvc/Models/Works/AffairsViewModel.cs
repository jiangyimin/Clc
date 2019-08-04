using System.Collections.Generic;

namespace Clc.Web.Models.Works
{
    public class AffairsViewModel
    {
        public string Today { get; set; }
        public int DepotId { get; set; }

        public List<string> PlaceNames { get; set; }
    }
}