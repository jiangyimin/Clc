using Abp.AutoMapper;

namespace Clc.Fields
{
    /// <summary>
    /// WorkerCacheItem
    /// </summary>
    [AutoMapFrom(typeof(Worker))]
    public class WorkerCacheItem
    {
        public int Id { get; set; }

        public int DepotId { get; set; }
        
        public string Cn { get; set; }

        public string Name { get; set; }

        public string Rfid { get; set; }
        public int PostId { get; set; }
        public string PostName { get; set; }

        public string WorkRoles { get; set; }

        public string Finger { get; set; }
        public string Finger2 { get; set; }

        public string CnNamePost { 
            get {
                return string.Format("{0} {1}({2})", Cn, Name, PostName);
            }
        }
    }
}

