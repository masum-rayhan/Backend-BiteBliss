using BiteBliss.DataAcces.Repo.IRepo;
using BiteBliss.DataAcces.Repo.IRepo.Auth;
using BiteBliss.DataAcces.Repo.IRepo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiteBliss.DataAccess.Repo.IRepo;

public interface IUnitOfWork
{
    Task SaveAsync();

    IBlobService BlobService { get; }
    IMenuItemRepo MenuItems { get; }
    IAuthRepo Auth { get; }
    IShoppingCartRepo ShoppingCart { get; }
    IOrderRepo Order { get; }
    IPaymentRepo Payments { get; }
}
