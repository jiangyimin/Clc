using System.Collections.Generic;
using System.Linq;
using Abp.Domain.Repositories;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using Clc.Fields.Entities;

namespace Clc.Fields.Cache
{
    public class ArticleCache : IArticleCache, IEventHandler<EntityChangedEventData<Article>>
    {
        private readonly string CacheName = "CachedArticle";
        private readonly IAbpSession _abpSession;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<Article> _articleRepository;

        public ArticleCache(
            ICacheManager cacheManager,
            IRepository<Article> articleRepository,
            IAbpSession abpSession)
        {
            _cacheManager = cacheManager;
            _articleRepository = articleRepository;
            _abpSession = abpSession;
        }

        public List<Article> GetList()
        {
            return _cacheManager.GetCache(CacheName)
                .Get(CacheKey, () => _articleRepository.GetAll().ToList());
        }

        public Article GetById(int id)
        {
            return GetList().FirstOrDefault(d => d.Id == id);
        }
        
        public Article GetByCn(string cn)
        {
            return GetList().FirstOrDefault(d => d.Cn == cn);
        }
        
        public void HandleEvent(EntityChangedEventData<Article> eventData)
        {
            _cacheManager.GetCache(CacheName).Remove(CacheKey);
        }

        private string CacheKey
        {
            get { return "Articles@" + (_abpSession.TenantId ?? 0); }
        }
    }
}