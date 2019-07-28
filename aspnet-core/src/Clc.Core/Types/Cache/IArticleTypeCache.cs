using System.Collections.Generic;
using Clc.Types.Entities;

namespace Clc.Types.Cache
{
    public interface IArticleTypeCache
    {
        List<ArticleType> GetList();

        ArticleType GetById(int id);

        ArticleType GetByCn(string cn);
    }
}