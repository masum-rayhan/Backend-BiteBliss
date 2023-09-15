﻿using BiteBliss.DataAcces.Data;
using BiteBliss.DataAccess.Repo.IRepo;
using BiteBliss.DataAccess.Repo.IRepo.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BiteBliss.DataAccess.Repo;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly AppDbContext _db;
    internal DbSet<T> dbSet;
    private readonly ICacheService _cacheService;
    public Repository(AppDbContext db, ICacheService cacheService)
    {
        _db = db;
        _cacheService = cacheService;
        this.dbSet = _db.Set<T>();
    }
    public async Task AddAsync(T entity)
    {
        await dbSet.AddAsync(entity);
    }

    public async Task UpdateAsync(T entity)
    {
        throw new NotImplementedException();
    }

    public async Task <T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter)
    {
        IQueryable<T> query = dbSet;
        query = query.Where(filter);

        return await query.FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        //IQueryable<T> query = dbSet;
        //return await query.ToListAsync();
        var cacheData = await _cacheService.GetData<IEnumerable<T>>(typeof(T).Name);
    }

    public async Task<T> GetDetailsAsync(int id)
    {
        var entity = await dbSet.FindAsync(id);
        return entity;
    }

    public void Remove(T entity)
    {
        dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entity)
    {
        dbSet.RemoveRange(entity);
    }
    public Task SaveAsync()
    {
        return _db.SaveChangesAsync();
    }
}
