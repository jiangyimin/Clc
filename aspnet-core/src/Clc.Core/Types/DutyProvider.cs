using System.Collections.Generic;
using Abp.Dependency;
using Abp.Runtime.Caching;
using Clc.Types.Cache;
using Clc.Types.Entities;

namespace Clc.Types
{
    /// <summary>
    /// Implements Typs Manager.
    /// </summary>
    public class DutyProvider : ITransientDependency
    {
        private IReadOnlyList<string> _dutyCategory;
        private Dictionary<string, IReadOnlyList<string>> _duties;

        public DutyProvider()
        {
            _dutyCategory = new List<string>() {"线路", "物品库", "调度室", "金库", "清分室"};
            
            _duties = new Dictionary<string, IReadOnlyList<string>>();
            _duties.Add("线路", new List<string>() {"网点交接"});

        }
        public IEnumerable<string> GetDutyCategory()
        {
            return _dutyCategory;
        }

        public IEnumerable<string> GetDuties(string category)
        {
            return _duties[category];
        }
    }
}
