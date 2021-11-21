using System;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Ninject.Extensions.Interception;
using Ninject.Extensions.Interception.Request;

namespace HotTowel.Web.Cache
{
    public class CacheInterceptor : ICacheInterceptor
    {
        private readonly ICacheProvider _cacheManager;
        //private readonly ICprProvider _cprProvider;

        public CacheInterceptor(ICacheProvider cacheManager
            //, ICprProvider cprProvider
        )
        {
            _cacheManager = cacheManager;
            //_cprProvider = cprProvider;
        }

        public TimeSpan? Timeout { get; set; }
        public string CacheKeyPrefix { get; set; }

        public void Intercept(IInvocation invocation)
        {
            var enableCaching = true; // _cprProvider.GetBooleanCprProperty("cache.enabled").IsTrue();

            if (enableCaching)
            {
                var minutes = GetTimeoutMinutes(invocation);
                var key = GenerateCacheKey(invocation.Request);

                invocation.ReturnValue = _cacheManager.GetOrAddCache(key, minutes, delegate
                {
                    invocation.Proceed();
                    return invocation.ReturnValue;
                });
            }
            else
            {
                invocation.Proceed();
            }
        }

        private int GetTimeoutMinutes(IInvocation invocation)
        {
            //get timeout set on the cache manager through ninject
            var minutes = _cacheManager.TimeoutMinutes;

            //get timeout from the decorator attribute at the class level
            if (Timeout.HasValue) minutes = Timeout.Value.Minutes;

            //get timeout from the decorator attribute at the method level
            var methodTimeouts = invocation.Request.Method.GetCustomAttributes(typeof(CacheTimeoutAttribute), false);
            if (methodTimeouts.Length > 0) minutes = ((CacheTimeoutAttribute)methodTimeouts.First()).Minutes;

            return minutes;
        }

        private string GenerateCacheKey(IProxyRequest request)
        {
            var sb = new StringBuilder();

            sb.Append(CacheKeyPrefix);
            sb.Append(".");
            sb.Append(request.Method.Name);
            sb.Append(".");

            foreach (var argument in request.Arguments)
            {
                sb.Append(JsonConvert.SerializeObject(argument).Replace("\"", "").Replace("{", "(").Replace("}", ")"));
                sb.Append(".");
            }

            sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }
    }
}