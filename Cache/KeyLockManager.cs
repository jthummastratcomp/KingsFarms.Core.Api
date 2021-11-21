using System.Collections.Generic;

namespace HotTowel.Web.Cache
{
    public class KeyLockManager
    {
        private static readonly object _syncRoot = new object();
        private readonly Dictionary<string, object> _keyLocks = new Dictionary<string, object>();

        public object AcquireKeyLock(string key)
        {
            lock (_syncRoot)
            {
                var obj = (object)null;
                if (_keyLocks.ContainsKey(key))
                    obj = _keyLocks[key];

                if (obj != null) return obj;
                obj = new object();
                _keyLocks.Add(key, obj);

                return obj;
            }
        }
    }
}