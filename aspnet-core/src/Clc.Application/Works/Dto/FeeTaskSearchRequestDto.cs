using System;

namespace Clc.Works.Dto
{
    public class FeeTaskSearchRequestDto
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public int? TaskTypeId { get; set; }
        public int? DepotId { get; set; }
        public int? VehicleId { get; set; }
        public int? CustomerId { get; set; }

        public int? OutletId { get; set; }
        public int? PriceLow { get; set; }
        public int? PriceHigh { get; set; }

    }
}
