using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiteBliss.DataAccess.Repo.IRepo.Services;

public interface ICacheService
{
    T GetData<T>(string key);
    bool SetData<T>(string key, T data, DateTimeOffset expirationTime);
    object RemoveData(string key);
}
