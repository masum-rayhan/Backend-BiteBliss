using BiteBliss.Models.DataTables;
using BiteBliss.Models.DataTables.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiteBliss.DataAcces.Repo.IRepo;

public interface IOrderRepo
{
    Task<OrderHeader> GetOrderAsync(string? userId);
    Task<OrderHeader> GetOrderAsync(int id);
    Task<OrderHeaderCreateDTO> CreateOrderAsync(OrderHeaderCreateDTO orderHeaderDTO);
    Task<OrderHeaderUpdateDTO> UpdateOrderHeaderAsync(int id, OrderHeaderUpdateDTO orderHeaderUpdateDTO);
}
