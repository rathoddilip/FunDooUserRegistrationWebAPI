using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class RedisCacheServiceBL
    {
        private readonly IDistributedCache distributedCache;

        public RedisCacheServiceBL(IDistributedCache distributedCache)
        {
            this.distributedCache = distributedCache;
        }

        public async Task RemoveNotesRedisCache(long Id)
        {
            var cacheKey = "ActiveNotes:" + Id.ToString();
            await distributedCache.RemoveAsync(cacheKey);
            cacheKey = "ArchiveNotes:" + Id.ToString();
            await distributedCache.RemoveAsync(cacheKey);
            cacheKey = "ReminderNotes:" + Id.ToString();
            await distributedCache.RemoveAsync(cacheKey);
        }
        public async Task AddRedisCache(string cacheKey, object obj)
        {
            string serializedNotes = JsonConvert.SerializeObject(obj);
            var redisNoteCollection = Encoding.UTF8.GetBytes(serializedNotes);
            var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(DateTime.Now.AddMinutes(20))
                .SetSlidingExpiration(TimeSpan.FromMinutes(2));
            await distributedCache.SetAsync(cacheKey, redisNoteCollection, options);
        }
    }
}
