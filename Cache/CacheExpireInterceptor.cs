using Ninject.Extensions.Interception;

namespace HotTowel.Web.Cache
{
    public class CacheExpireInterceptor : ICacheExpireInterceptor
    {
        private readonly ICacheProvider _cacheManager;

        public CacheExpireInterceptor(ICacheProvider cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public string CacheKeyPrefix { get; set; }

        public void Intercept(IInvocation invocation)
        {
            _cacheManager.RemoveCache(CacheKeyPrefix);
        }
    }
}