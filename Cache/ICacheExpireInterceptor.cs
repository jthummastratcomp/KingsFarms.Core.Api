using Ninject.Extensions.Interception;

namespace HotTowel.Web.Cache
{
    public interface ICacheExpireInterceptor : IInterceptor
    {
        string CacheKeyPrefix { get; set; }
    }
}