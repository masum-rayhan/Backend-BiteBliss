using BiteBliss.DataAccess.Repo.IRepo;
using BiteBliss.Models.DataTables;
using BiteBliss.Models.DataTables.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiteBliss.DataAcces.Repo.IRepo;

public interface IMenuItemRepo : IRepository<MenuItem>
{
    Task<MenuItem> CreateMenuItemAsync(MenuItemCreateDTO menuItemCreateDTO);
    Task<MenuItem> UpdateMenuItemAsync(int id, MenuItemUpdateDTO menuItemUpdateDTO);
    Task<bool> DeleteMenuItemAsync(int id);
}
