using System;
using System.Web;

namespace Timetracker.Controllers
{
    public class InMemoryCache 
    {
        public T Get<T>(string cacheId) where T : class
        {
            T item = HttpRuntime.Cache.Get(cacheId) as T;
            return item;
        }

        public T Get<T>(string cacheId, Func<T> getItemCallback) where T : class
        {
            T item = HttpRuntime.Cache.Get(cacheId) as T;
            if (item != null)
                return item;
            item = getItemCallback();
            HttpContext.Current.Cache.Insert(cacheId, item);
            return item;
        }

        public void Add(string cacheKey, object data)
        {
            HttpContext.Current.Cache.Insert(cacheKey, data);
        }
    }
}