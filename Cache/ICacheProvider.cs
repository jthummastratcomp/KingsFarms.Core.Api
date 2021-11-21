using System;

namespace HotTowel.Web.Cache
{
    public interface ICacheProvider
    {
        int TimeoutMinutes { get; set; }
        T GetOrAddCache<T>(string key, int timeoutMinutes, Func<T> fn) where T : class;
        void RemoveCache(string key);
    }
}