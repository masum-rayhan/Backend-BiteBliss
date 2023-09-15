using BiteBliss.DataAccess.Repo.IRepo.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BiteBliss.DataAccess.Repo.Services;

public class CacheService : ICacheService
{
    private IDatabase _cacheDb;
    public CacheService()
    {
        var redis = ConnectionMultiplexer.Connect("localhost:6379");
        _cacheDb = redis.GetDatabase();
    }

    public async Task<T> GetDataAsync<T>(string key)
    {
        var value = await _cacheDb.StringGetAsync(key);
        if(!string.IsNullOrEmpty(value))
            return JsonSerializer.Deserialize<T>(value);
        return default;
    }

    public async Task<bool> RemoveDataAsync(string key)
    {
        var _exist = await _cacheDb.KeyExistsAsync(key);

        if (_exist)
            return await _cacheDb.KeyDeleteAsync(key);

        return false;
    }

    public async Task<bool> SetDataAsync<T>(string key, T data, DateTimeOffset expirationTime)
    {
        var expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
        return await _cacheDb.StringSetAsync(key, JsonSerializer.Serialize(data), expiryTime);
    }
}
