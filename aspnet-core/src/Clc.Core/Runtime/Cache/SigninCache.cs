using System;
using System.Linq;
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

        public Signin Get(int depotId, int workerId, bool isMorning)
        {
            string cacheKey = depotId.ToString() + DateTime.Now.Date.ToString();
            var list = _cacheManager.GetCache(CacheName).Get(cacheKey, () => {
                return _signinRepository.GetAll()
                    .Where(x => x.CarryoutDate == DateTime.Now.Date).ToList();
            });
            
            return list.LastOrDefault(x => x.WorkerId == workerId && ClcUtils.IsMorning(x.SigninTime) == isMorning);
        }
    }
}