using System;
using Ninject.Extensions.Interception;

namespace HotTowel.Web.Cache
{
    public interface ICacheInterceptor : IInterceptor
    {
        TimeSpan? Timeout { get; set; }
        string CacheKeyPrefix { get; set; }
    }
}