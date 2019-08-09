using Abp.AutoMapper;

namespace Clc.Fields
{
    /// <summary>
    /// WorkerListItem
    /// </summary>
    [AutoMapFrom(typeof(Worker))]
    public class WorkerListItem
    {
        public int Id { get; set; }

        public int DepotId { get; set; }
        public int? LoanDepotId { get; set; }

        public string Cn { get; set; }

        public string Name { get; set; }

        public int PostId { get; set; }

        public string CnName { 
            get {
                return string.Format("{0} {1}", Cn, Name);
            }
        }
    }
}

