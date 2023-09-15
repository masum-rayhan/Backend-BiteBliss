using BiteBliss.Models.DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiteBliss.DataAcces.Repo.IRepo;

public interface IPaymentRepo
{
    Task<ShoppingCart> CreatePaymentAsync(string userId);
}
