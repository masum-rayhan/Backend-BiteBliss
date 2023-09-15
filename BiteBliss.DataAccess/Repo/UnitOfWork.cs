using Azure.Storage.Blobs;
using BiteBliss.DataAcces.Data;
using BiteBliss.DataAcces.Repo;
using BiteBliss.DataAcces.Repo.Auth;
using BiteBliss.DataAcces.Repo.IRepo;
using BiteBliss.DataAcces.Repo.IRepo.Auth;
using BiteBliss.DataAcces.Repo.IRepo.Services;
using BiteBliss.DataAcces.Repo.Services;
using BiteBliss.DataAccess.Repo.IRepo;
using BiteBliss.DataAccess.Repo.IRepo.Services;
using BiteBliss.DataAccess.Repo.Services;
using BiteBliss.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiteBliss.DataAccess.Repo;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _db;
    private readonly BlobService _blobService;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _config;
    private string secretKey;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICacheService _cacheService;

    public UnitOfWork(AppDbContext db, BlobService blobService,
        RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, 
        IConfiguration config, ICacheService cacheService)
    {
        _db = db;
        _blobService = blobService;
        _roleManager = roleManager;
        _userManager = userManager;
        _config = config;
        _cacheService = cacheService;
    }

    public Task SaveAsync()
    {
        return _db.SaveChangesAsync();
    }

    public IMenuItemRepo MenuItems => new MenuItemRepo(_db, _blobService, _cacheService);

    public IBlobService BlobService => _blobService;

    public IAuthRepo Auth => new AuthRepo(_db, _config, _roleManager, _userManager);

    public IShoppingCartRepo ShoppingCart => new ShoppingCartRepo(_db);

    public IOrderRepo Order => new OrderRepo(_db);

    public IPaymentRepo Payments => new PaymentRepo(_db, _config);

    public ICacheService Cache => new CacheService();
}
