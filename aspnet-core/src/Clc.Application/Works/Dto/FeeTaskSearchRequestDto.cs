using System;

namespace Clc.Works.Dto
{
    public class FeeTaskSearchRequestDto
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int? DepotId { get; set; }
        public int? CustomerId { get; set; }

    }
}
