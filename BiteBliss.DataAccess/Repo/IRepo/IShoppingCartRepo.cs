using BiteBliss.Models.DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiteBliss.DataAcces.Repo.IRepo;

public interface IShoppingCartRepo
{
    Task <ShoppingCart> GetAsync (string userId);
    Task<ShoppingCart> CreateOrUpdateItemInCartAsync(string userId, int menuItemId, int updateQuantityBy); 
}
