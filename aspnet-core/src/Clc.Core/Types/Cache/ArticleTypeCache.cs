using System;
using System.Collections.Generic;
using System.Linq;
using Abp;
using Abp.Domain.Repositories;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using Clc.Types.Entities;

namespace Clc.Types.Cache
{
    public class ArticleTypeCache : IArticleTypeCache, IEventHandler<EntityChangedEventData<ArticleType>>
    {
        private readonly string CacheName = "CachedArticleType";
        private readonly IAbpSession _abpSession;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<ArticleType> _articleTypeRepository;

        public ArticleTypeCache(
            ICacheManager cacheManager,
            IRepository<ArticleType> articleTypeRepository,
            IAbpSession abpSession)
        {
            _cacheManager = cacheManager;
            _articleTypeRepository = articleTypeRepository;
            _abpSession = abpSession;

            ICache cache = _cacheManager.GetCache(CacheName);
            cache.DefaultSlidingExpireTime = TimeSpan.FromHours(ClcConsts.TypeCacheSlidingExpireTime);
        }

        public List<ArticleType> GetList()
        {
            var cacheKey = "ArticleTypes@" + (_abpSession.TenantId ?? 0);
            return _cacheManager.GetCache(CacheName)
                .Get(cacheKey, () => _articleTypeRepository.GetAll().ToList());
        }

        public ArticleType GetById(int id)
        {
            return GetList().FirstOrDefault(d => d.Id == id);
        }
        
        public ArticleType GetByCn(string cn)
        {
            return GetList().FirstOrDefault(d => d.Cn == cn);
        }
        
        public void HandleEvent(EntityChangedEventData<ArticleType> eventData)
        {
            _cacheManager.GetCache(CacheName).Remove(CacheKey);
        }

        private string CacheKey
        {
            get { return "ArticleTypes@" + (_abpSession.TenantId ?? 0); }
        }
    }
}