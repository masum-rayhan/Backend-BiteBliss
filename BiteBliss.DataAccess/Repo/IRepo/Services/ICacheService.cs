using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiteBliss.DataAccess.Repo.IRepo.Services;

public interface ICacheService
{
    T GetDataAsync<T>(string key);
    bool SetDataAsync<T>(string key, T data, DateTimeOffset expirationTime);
    bool RemoveDataAsync(string key);
}
