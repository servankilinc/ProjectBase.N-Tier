using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace Core.Utils.Caching;

public class CacheService : ICacheService
{
    private readonly IDistributedCache _distributedCache;
    public CacheService(IDistributedCache distributedCache) => _distributedCache = distributedCache;

    public CacheResponse GetFromCache(string cacheKey)
    {
        if (string.IsNullOrWhiteSpace(cacheKey)) throw new ArgumentNullException(nameof(cacheKey));

        byte[]? cachedData = _distributedCache.Get(cacheKey);
        if (cachedData != null)
        {
            var response = Encoding.UTF8.GetString(cachedData);
            if (string.IsNullOrEmpty(response)) return new CacheResponse(IsSuccess: false);

            return new CacheResponse(IsSuccess: true, Source: response);
        }
        else
        {
            return new CacheResponse(IsSuccess: false);
        }
    }

    public void AddToCache<TData>(string cacheKey, string[] cacheGroupKeys, TData data)
    {
        if (string.IsNullOrWhiteSpace(cacheKey)) throw new ArgumentNullException(nameof(cacheKey));

        DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions()
        {
            SlidingExpiration = TimeSpan.FromHours(2),
            AbsoluteExpiration = DateTime.UtcNow.AddMinutes(30)
        };

        string serializedData = JsonConvert.SerializeObject(data, new JsonSerializerSettings
        {
            Formatting = Formatting.None,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            MaxDepth = 7
        });
        byte[]? bytedData = Encoding.UTF8.GetBytes(serializedData);

        _distributedCache.Set(cacheKey, bytedData, cacheEntryOptions);

        if (cacheGroupKeys != null && cacheGroupKeys.Any()) AddCacheKeyToGroups(cacheKey, cacheGroupKeys, cacheEntryOptions);
    }

    public void RemoveFromCache(string cacheKey)
    {
        if (string.IsNullOrWhiteSpace(cacheKey)) throw new ArgumentNullException(nameof(cacheKey));

        _distributedCache.Remove(cacheKey);
    }

    public void RemoveCacheGroupKeys(string[] cacheGroupKeyList)
    {
        if (cacheGroupKeyList == null) throw new ArgumentNullException(nameof(cacheGroupKeyList));

        foreach (string cacheGroupKey in cacheGroupKeyList)
        {
            byte[]? keyListFromCache = _distributedCache.Get(cacheGroupKey);
            _distributedCache.Remove(cacheGroupKey);

            if (keyListFromCache == null) continue;

            string stringKeyList = Encoding.UTF8.GetString(keyListFromCache);
            HashSet<string>? keyListInGroup = JsonConvert.DeserializeObject<HashSet<string>>(stringKeyList);
            if (keyListInGroup != null)
            {
                foreach (var key in keyListInGroup)
                {
                    _distributedCache.Remove(key);
                }
            }
        }
    }

    private void AddCacheKeyToGroups(string cacheKey, string[] cacheGroupKeys, DistributedCacheEntryOptions groupCacheEntryOptions)
    {
        foreach (string cacheGroupKey in cacheGroupKeys)
        {
            HashSet<string>? keyListInGroup;
            byte[]? cachedGroupData = _distributedCache.Get(cacheGroupKey);
            if (cachedGroupData != null)
            {
                keyListInGroup = JsonConvert.DeserializeObject<HashSet<string>>(Encoding.UTF8.GetString(cachedGroupData));
                if (keyListInGroup != null && !keyListInGroup.Contains(cacheKey))
                {
                    keyListInGroup.Add(cacheKey);
                }
            }
            else
            {
                keyListInGroup = new HashSet<string>(new[] { cacheKey });
            }
            string serializedData = JsonConvert.SerializeObject(keyListInGroup, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                MaxDepth = 7
            });
            byte[]? bytedKeyList = Encoding.UTF8.GetBytes(serializedData);
            _distributedCache.Set(cacheGroupKey, bytedKeyList, groupCacheEntryOptions);
        }
    }
}