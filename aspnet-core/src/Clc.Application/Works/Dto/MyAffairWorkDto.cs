using System;
using System.Collections.Generic;

namespace Clc.Works.Dto
{
    /// <summary>
    /// MyAffairWork Model
    /// </summary>
    public class MyAffairWorkDto
    {
        public DateTime Now { get; set; }
        public string Today { get; set; }
        public int DepotId { get; set; }
        public int AffairId { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}

