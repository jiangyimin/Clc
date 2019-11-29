using System.Collections.Generic;

namespace Clc.Web.Models.Weixin
{
    public class AskDoorViewModel
    {
        public int RecordId { get; set; }
        public string WorkplaceName { get; set; }

        public int WorkerId { get; set; }
        
        public AskDoorViewModel()
        {
        }
    }

}