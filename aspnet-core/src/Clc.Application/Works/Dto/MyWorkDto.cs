using System;
using System.Collections.Generic;

namespace Clc.Works.Dto
{
    /// <summary>
    /// MyWorkModel
    /// </summary>
    public class MyWorkDto
    {
        // Me
        public string WorkerCn { get; set; }        
        
        public int AffairId { get; set; }

        public string Status { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public List<MyWorkerDto> Workers { get; set; }

    }

    public class MyWorkerDto
    {
        public string Cn { get; set; }
        public string Name { get; set; }
        public string Rfid { get; set; }
        public string Photo { get; set; }
    }
}

