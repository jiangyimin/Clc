using System.Collections.Generic;
using Clc.Types.Entities;

namespace Clc.Types.Cache
{
    public interface IPostCache
    {
        List<Post> GetList();

        Post GetById(int id);
    }
}