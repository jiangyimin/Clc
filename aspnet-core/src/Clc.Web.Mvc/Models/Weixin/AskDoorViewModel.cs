using System.Collections.Generic;

namespace Clc.Web.Models.Weixin
{
    public class AskDoorViewModel
    {
        public int AffairId { get; set; }
        public int WorkplaceId { get; set; }

        public List<ComboItemModel> Workplaces {get; set; }

    }
}