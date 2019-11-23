using System;
using System.Linq;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Caching;

namespace Clc.Runtime.Cache
{
    public class SigninCache : ISigninCache, ITransientDependency, IEventHandler<EntityCreatedEventData<Signin>> 
    {
        private readonly string CacheName = "CachedSignin";
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<Signin> _signinRepository;

        public SigninCache(
            ICacheManager cacheManager,
            IRepository<Signin> signinRepository)
        {
            _cacheManager = cacheManager;
            _signinRepository = signinRepository;
            _cacheManager.GetCache(CacheName).DefaultSlidingExpireTime = TimeSpan.FromHours(6);
        }

        public Signin Get(int depotId, int workerId, bool isMorning)
        {
            string cacheKey = DateTime.Now.Date.ToString() + depotId.ToString() + workerId.ToString();
            var signin = _cacheManager.GetCache(CacheName).Get(cacheKey, () => {
                return _signinRepository.GetAll()
                    .Where(x => x.CarryoutDate == DateTime.Now.Date && x.DepotId == depotId && x.WorkerId == workerId
                        && ClcUtils.IsMorning(x.SigninTime) == isMorning).FirstOrDefault();
            });
            
            return signin;
        }

        public void HandleEvent(EntityCreatedEventData<Signin> eventData)
        {
            var cacheKey = DateTime.Now.Date.ToString() + eventData.Entity.DepotId.ToString() + eventData.Entity.Id.ToString();
            _cacheManager.GetCache(CacheName).Remove(cacheKey);
        }

    }
}