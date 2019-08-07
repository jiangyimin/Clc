using System.Collections.Generic;

namespace Clc.Runtime
{
    public interface IEntityListCache<TEntity, TCacheItem, TListItem>
    {
        TCacheItem this[int id] { get; }
        
        TCacheItem Get(int id);

        List<TListItem> GetList();

    }
}