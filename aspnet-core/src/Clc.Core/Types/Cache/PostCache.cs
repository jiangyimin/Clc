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
    public class PostCache : IPostCache, IEventHandler<EntityChangedEventData<Post>>
    {
        private readonly string CacheName = "CachedPost";
        private readonly IAbpSession _abpSession;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<Post> _postRepository;

        public PostCache(
            ICacheManager cacheManager,
            IRepository<Post> postRepository,
            IAbpSession abpSession)
        {
            _cacheManager = cacheManager;
            _postRepository = postRepository;
            _abpSession = abpSession;

            ICache cache = _cacheManager.GetCache(CacheName);
            cache.DefaultSlidingExpireTime = TimeSpan.FromHours(ClcConsts.TypeCacheSlidingExpireTime);
        }

        public List<Post> GetList()
        {
            var cacheKey = "Posts@" + (_abpSession.TenantId ?? 0);
            return _cacheManager.GetCache(CacheName)
                .Get(cacheKey, () => _postRepository.GetAll().ToList());
        }

        public Post GetById(int id)
        {
            return GetList().FirstOrDefault(d => d.Id == id);
        }
        
        public void HandleEvent(EntityChangedEventData<Post> eventData)
        {
            _cacheManager.GetCache(CacheName).Remove(CacheKey);
        }

        private string CacheKey
        {
            get { return "Posts@" + (_abpSession.TenantId ?? 0); }
        }
    }
}