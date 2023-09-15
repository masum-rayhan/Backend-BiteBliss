using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BiteBliss.DataAccess.Repo.IRepo;

public interface IRepository<T> where T : class
{
    Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entity);
    Task <T> GetDetailsAsync(int id);
    Task SaveAsync();
}
