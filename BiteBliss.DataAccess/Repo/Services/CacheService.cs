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

    public T GetData<T>(string key)
    {
        var value = _cacheDb.StringGet(key);
        if(!string.IsNullOrEmpty(value))
            return JsonSerializer.Deserialize<T>(value);
        return default;
    }

    public object RemoveData(string key)
    {
        var _exist = _cacheDb.KeyExists(key);

        if (_exist)
            return _cacheDb.KeyDelete(key);

        return false;
    }

    public bool SetData<T>(string key, T data, DateTimeOffset expirationTime)
    {
        var expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
        return _cacheDb.StringSet(key, JsonSerializer.Serialize(data), expiryTime);
    }
}
