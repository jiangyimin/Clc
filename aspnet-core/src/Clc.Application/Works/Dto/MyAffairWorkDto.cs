using System;
using System.Collections.Generic;
using Clc.Affairs;

namespace Clc.Works.Dto
{
    /// <summary>
    /// MyAffairWork Model
    /// </summary>
    public class MyAffairWorkDto
    {
        public bool Alt { get; set; }
        public string Today { get; set; }
        public int DepotId { get; set; }
        public int AffairId { get; set; }
        public string WorkplaceName { get; set; }

        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public MyAffairWorkDto SetAffair(AffairCacheItem affair, string wpName, bool alt)
        {
            Alt = alt;
            Today = DateTime.Now.ToString("yyyy-MM-dd");
            if (affair != null)
            {
                AffairId = affair.Id;
                WorkplaceName = wpName;
                StartTime = affair.StartTime;
                EndTime = affair.EndTime;
            }
            return this;
        }
    }
}

