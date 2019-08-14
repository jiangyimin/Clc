using System;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Runtime.Caching;

namespace Clc.Runtime.Cache
{
    public class SigninCache : ISigninCache, ITransientDependency
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
            _cacheManager.GetCache(CacheName).DefaultSlidingExpireTime = TimeSpan.FromHours(12);
        }

        public Signin Get(int depotId, int workerId)
        {
            string cacheKey = depotId.ToString() + workerId.ToString() + DateTime.Now.Date.ToString();
            return _cacheManager.GetCache(CacheName)
                .Get(cacheKey, () => _signinRepository.FirstOrDefault(x => x.DepotId == depotId && x.CarryoutDate == DateTime.Now.Date && x.WorkerId == workerId));
        }
    }
}