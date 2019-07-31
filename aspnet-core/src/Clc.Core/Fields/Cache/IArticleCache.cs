using System.Collections.Generic;
using Clc.Fields.Entities;

namespace Clc.Fields.Cache
{
    public interface IArticleCache
    {
        List<Article> GetList();

        Article GetById(int id);
        Article GetByCn(string cn);
    }
}