using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MakeVolunteerGreatAgain.Infrastructure.Services.Redis
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;

        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task SetCacheValueAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiry ?? TimeSpan.FromMinutes(5)
            };
            var jsonData = JsonSerializer.Serialize(value);
            await _cache.SetStringAsync(key, jsonData, options);
        }

        public async Task<T> GetCacheValueAsync<T>(string key)
        {
            var jsonData = await _cache.GetStringAsync(key);
            return jsonData != null ? JsonSerializer.Deserialize<T>(jsonData) : default;
        }

        public async Task RemoveCacheValueAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }
    }


    public interface ICacheService
    {
        Task SetCacheValueAsync<T>(string key, T value, TimeSpan? expiry = null);
        Task<T> GetCacheValueAsync<T>(string key);
        Task RemoveCacheValueAsync(string key);
    }

}
