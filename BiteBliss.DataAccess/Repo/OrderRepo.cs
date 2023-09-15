using BiteBliss.DataAcces.Data;
using BiteBliss.DataAcces.Repo.IRepo;
using BiteBliss.DataAcces.Utils;
using BiteBliss.Models.DataTables;
using BiteBliss.Models.DataTables.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiteBliss.DataAcces.Repo;

public class OrderRepo : IOrderRepo
{
    private readonly AppDbContext _db;
    public OrderRepo(AppDbContext db)
    {
        _db = db;
    }

    public async Task<OrderHeader> GetOrderAsync(string? userId)
    {
        try
        {
            var orderHeaders = _db.OrderHeaders
                            .Include(u => u.OrderDetails)
                            .ThenInclude(u => u.MenuItem)
                            .OrderByDescending(u => u.OrderHeaderId);

            if (!string.IsNullOrEmpty(userId))
            {
                var user = orderHeaders.Where(u => u.ApplicationUserId == userId);

                return await user.FirstOrDefaultAsync();
            }
            else
                return await orderHeaders.FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            throw new ApplicationException("An error occurred while processing the request", ex);
        }
    }

    public async Task<OrderHeader> GetOrderAsync(int id)
    {
        try
        {
            if (id == 0)
            {
                throw new ArgumentException("Order Id is null or empty");
            }

            var orderHeaders = _db.OrderHeaders
                            .Include(u => u.OrderDetails)
                            .ThenInclude(u => u.MenuItem)
                            .Where(u => u.OrderHeaderId == id);

            if (orderHeaders == null)
            {
                throw new ArgumentException("Order not found");
            }

            return await orderHeaders.FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            throw new ApplicationException("An error occurred while processing the request", ex);
        }
    }

    public async Task<OrderHeaderCreateDTO> CreateOrderAsync(OrderHeaderCreateDTO orderHeaderDTO)
    {
        try
        {
            OrderHeader newOrder = new()
            {
                ApplicationUserId = orderHeaderDTO.ApplicationUserId,
                PickupEmail = orderHeaderDTO.PickupEmail,
                PickupName = orderHeaderDTO.PickupName,
                PickupPhoneNumber = orderHeaderDTO.PickupPhoneNumber,
                OrderTotal = orderHeaderDTO.OrderTotal,
                OrderDate = DateTime.Now,
                StripePaymentIntentId = orderHeaderDTO.StripePaymentIntentId,
                TotalItems = orderHeaderDTO.TotalItems,
                PaymentStatus = String.IsNullOrEmpty(orderHeaderDTO.Status) ? SD.Status_Pending : orderHeaderDTO.Status,
            };

            _db.OrderHeaders.Add(newOrder);
            _db.SaveChanges();

            foreach (var orderDetailsDTO in orderHeaderDTO.OrderDetailsDTO)
            {
                OrderDetails newOrderDetails = new()
                {
                    OrderHeaderId = newOrder.OrderHeaderId,
                    ItemName = orderDetailsDTO.ItemName,
                    MenuItemId = orderDetailsDTO.MenuItemId,
                    Price = orderDetailsDTO.Price,
                    Quantity = orderDetailsDTO.Quantity,
                };
                _db.OrderDetails.Add(newOrderDetails);
            }
            _db.SaveChanges();

            return orderHeaderDTO;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("An error occurred while processing the request", ex);
        }
    }

    public async Task<OrderHeaderUpdateDTO> UpdateOrderHeaderAsync(int id, OrderHeaderUpdateDTO orderHeaderUpdateDTO)
    {
        try
        {
            if (orderHeaderUpdateDTO == null || id != orderHeaderUpdateDTO.OrderHeaderId)
                throw new ArgumentException("Order Header is null or empty");

            var orderFromDb = await _db.OrderHeaders.FindAsync(id);

            if (orderFromDb == null)
                throw new ArgumentException("Order not found");

            if(!String.IsNullOrEmpty(orderHeaderUpdateDTO.PickupName))
                orderFromDb.PickupName = orderHeaderUpdateDTO.PickupName;
            if (!String.IsNullOrEmpty(orderHeaderUpdateDTO.PickupPhoneNumber))
                orderFromDb.PickupPhoneNumber = orderHeaderUpdateDTO.PickupPhoneNumber;
            if (!String.IsNullOrEmpty(orderHeaderUpdateDTO.PickupEmail))
                orderFromDb.PickupEmail = orderHeaderUpdateDTO.PickupEmail;
            if (!String.IsNullOrEmpty(orderHeaderUpdateDTO.Status))
                orderFromDb.PaymentStatus = orderHeaderUpdateDTO.Status;
            if(!String.IsNullOrEmpty(orderHeaderUpdateDTO.StripePaymentIntentId))
                orderFromDb.StripePaymentIntentId = orderHeaderUpdateDTO.StripePaymentIntentId;

            _db.OrderHeaders.Update(orderFromDb);
            _db.SaveChanges();

            return orderHeaderUpdateDTO;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("An error occurred while processing the request", ex);
        }
    }
}
