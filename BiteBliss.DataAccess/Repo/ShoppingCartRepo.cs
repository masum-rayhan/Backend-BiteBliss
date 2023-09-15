using BiteBliss.DataAcces.Data;
using BiteBliss.DataAcces.Repo.IRepo;
using BiteBliss.Models.DataTables;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiteBliss.DataAcces.Repo;

public class ShoppingCartRepo : IShoppingCartRepo
{
    private readonly AppDbContext _db;
    public ShoppingCartRepo(AppDbContext db)
    {
        _db = db;
    }
    public async Task<ShoppingCart> GetAsync(string userId)
    {
        try
        {
            ShoppingCart shoppingCart;
            if (string.IsNullOrEmpty(userId))
            {
                shoppingCart = new();
            }
            else
            {
                 shoppingCart = await _db.ShoppingCarts
                .Include(u => u.CartItems)
                .ThenInclude(u => u.MenuItem).FirstOrDefaultAsync(u => u.UserId == userId);
            }

            if (shoppingCart?.CartItems != null && shoppingCart.CartItems.Count() > 0)
                shoppingCart.CartTotal = shoppingCart.CartItems.Sum(u => u.MenuItem.Price * u.Quantity);

            return shoppingCart;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("An error occurred while processing the request", ex);
        }
    }

    public async Task<ShoppingCart> CreateOrUpdateItemInCartAsync(string userId, int menuItemId, int updateQuantityBy)
    {
        try
        {
            var shoppingCart = _db.ShoppingCarts.Include(u => u.CartItems).FirstOrDefault(u => u.UserId == userId);
            var manuItem = await _db.MenuItems.FindAsync(menuItemId);

            if (manuItem == null)
                throw new ArgumentException("Menu item not found");

            if (shoppingCart == null && updateQuantityBy > 0)
            {
                //create a shopping cart & add cart item
                ShoppingCart newCart = new() { UserId = userId };

                _db.ShoppingCarts.Add(newCart);
                await _db.SaveChangesAsync();

                CartItem newCartItem = new()
                {
                    MenuItemId = menuItemId,
                    Quantity = updateQuantityBy,
                    ShoppingCartId = newCart.Id,
                    MenuItem = null
                };

                _db.CartItems.Add(newCartItem);
                await _db.SaveChangesAsync();

                return newCart;
            }
            else
            {
                //shopping cart exists
                var cartItemInCart = shoppingCart?.CartItems.FirstOrDefault(u => u.MenuItemId == menuItemId);
                if (cartItemInCart == null)
                {
                    //add cart item
                    CartItem newCartItem = new()
                    {
                        MenuItemId = menuItemId,
                        Quantity = updateQuantityBy,
                        ShoppingCartId = shoppingCart.Id,
                        MenuItem = null
                    };
                    _db.CartItems.Add(newCartItem);
                    await _db.SaveChangesAsync();
                }
                else
                {
                    //item exists in cart, update quantity
                    var newQuantity = cartItemInCart.Quantity += updateQuantityBy;
                    if (updateQuantityBy == 0 || cartItemInCart.Quantity <= 0)
                    {
                        //remove item from cart
                        _db.CartItems.Remove(cartItemInCart);

                        if (shoppingCart.CartItems.Count() == 1)
                            _db.ShoppingCarts.Remove(shoppingCart);

                        await _db.SaveChangesAsync();

                        return shoppingCart;
                    }
                    else
                    {
                        cartItemInCart.Quantity = newQuantity;
                        await _db.SaveChangesAsync();

                        return shoppingCart;
                    }

                    return shoppingCart;
                }

            }

            return shoppingCart;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("An error occurred while processing the request", ex);
        }
    }
}
