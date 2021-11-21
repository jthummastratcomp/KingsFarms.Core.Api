using System;
using System.Diagnostics;
using LazyCache;
using Microsoft.Extensions.Caching.Memory;

namespace HotTowel.Web.Cache
{
    public class MemoryCacheProvider : ICacheProvider
    {
        //private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public MemoryCacheProvider(int cacheTimeoutMinutes)
        {
            TimeoutMinutes = cacheTimeoutMinutes;
        }

        public int TimeoutMinutes { get; set; }

        public T GetOrAddCache<T>(string key, int timeoutMinutes, Func<T> fn) where T : class
        {
            IAppCache cache = new CachingService();
            //var cachePolicy = new CacheItemPolicy 
            //{
            //    RemovedCallback = OnCacheExpired,
            //    SlidingExpiration = new TimeSpan(0, Convert.ToInt32(timeoutMinutes), 0)
            //};
            var cachePolicy = new MemoryCacheEntryOptions
            {
                SlidingExpiration = new TimeSpan(0, Convert.ToInt32(timeoutMinutes), 0)
            }.RegisterPostEvictionCallback(
                (key1, value, reason, state) => Trace.WriteLine($"Removing {key1} from Cache")
            );
            var cachedItem = cache.Get<T>(key);
            if (cachedItem != null)
            {
                Trace.WriteLine($"Retrieving {key} from Cache");
                return cachedItem;
            }

            Trace.WriteLine($"Adding {key} to Cache");
            return cache.GetOrAdd(key, fn, cachePolicy);
        }

        public void RemoveCache(string key)
        {
            IAppCache cache = new CachingService();
            cache.Remove(key);
        }

        //private void OnCacheExpired(CacheEntryRemovedArguments arguments)
        //{
        //    Trace.WriteLine($"Removing {arguments.CacheItem.Key} from Cache");
        //}
    }
}