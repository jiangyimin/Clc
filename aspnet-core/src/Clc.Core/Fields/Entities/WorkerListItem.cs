using Abp.AutoMapper;

namespace Clc.Fields.Entities
{
    /// <summary>
    /// WorkerListItem
    /// </summary>
    [AutoMapFrom(typeof(Worker))]
    public class WorkerListItem
    {
        public int Id { get; set; }

        public int DepotId { get; set; }

        public string Cn { get; set; }

        public string Name { get; set; }

        public int PostId { get; set; }
    }
}

