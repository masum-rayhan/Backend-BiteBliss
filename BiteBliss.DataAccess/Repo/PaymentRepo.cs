using BiteBliss.DataAcces.Data;
using BiteBliss.DataAcces.Repo.IRepo;
using BiteBliss.Models.DataTables;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiteBliss.DataAcces.Repo;

public class PaymentRepo : IPaymentRepo
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _configuration;

    public PaymentRepo(AppDbContext db, IConfiguration configuration)
    {
        _db = db;
        _configuration = configuration;
    }

    public async Task<ShoppingCart> CreatePaymentAsync(string userId)
    {
        var shoppingCart = _db.ShoppingCarts
            .Include(x => x.CartItems)
            .ThenInclude(x => x.MenuItem).FirstOrDefault(x => x.UserId == userId);

        if (shoppingCart == null || shoppingCart.CartItems == null || shoppingCart.CartItems.Count() == 0)
            throw new Exception("Shopping cart is empty");

        #region Create Payment Intent
        
        StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];
        shoppingCart.CartTotal = shoppingCart.CartItems.Sum(x => x.Quantity * x.MenuItem.Price);

        PaymentIntentCreateOptions options = new()
        {
            Amount = (int)(shoppingCart.CartTotal * 100),
            Currency = "usd",
            PaymentMethodTypes = new List<string>
            {
                "card"
            },
        };
        PaymentIntentService service = new();
        PaymentIntent response = service.Create(options);

        shoppingCart.StripePaymentIntentId = response.Id;
        shoppingCart.ClientSecret = response.ClientSecret;
        #endregion

        return shoppingCart;
    }
}
