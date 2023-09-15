using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiteBliss.DataAccess.Repo.IRepo.Services;

public interface ICacheService
{
    Task<T> GetDataAsync<T>(string key);
    Task<bool> SetDataAsync<T>(string key, T data, DateTimeOffset expirationTime);
    Task<bool> RemoveDataAsync(string key);
}
