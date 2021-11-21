using System;
using Ninject;
using Ninject.Extensions.Interception;
using Ninject.Extensions.Interception.Attributes;
using Ninject.Extensions.Interception.Request;

namespace HotTowel.Web.Cache
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class CacheTimeoutAttribute : InterceptAttribute
    {
        public int Minutes { get; set; }
        public string CacheKey { get; set; }

        public override IInterceptor CreateInterceptor(IProxyRequest request)
        {
            var interceptor = request.Kernel.Get<ICacheInterceptor>();
            if (interceptor == null) return null;

            if (Minutes != 0) interceptor.Timeout = TimeSpan.FromMinutes(Minutes);
            interceptor.CacheKeyPrefix = string.IsNullOrEmpty(CacheKey) ? request.Target.GetType().Name : CacheKey;

            return interceptor;
        }
    }
}