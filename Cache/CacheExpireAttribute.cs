using System;
using Ninject;
using Ninject.Extensions.Interception;
using Ninject.Extensions.Interception.Attributes;
using Ninject.Extensions.Interception.Request;

namespace HotTowel.Web.Cache
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class CacheExpireAttribute : InterceptAttribute
    {
        public string CacheKey { get; set; }

        public override IInterceptor CreateInterceptor(IProxyRequest request)
        {
            var interceptor = request.Kernel.Get<ICacheExpireInterceptor>();
            if (interceptor == null) return null;

            interceptor.CacheKeyPrefix = CacheKey; //request.Target.GetType().FullName;

            return interceptor;
        }
    }
}