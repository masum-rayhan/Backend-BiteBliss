using BiteBliss.DataAccess.Repo.IRepo;
using BiteBliss.Models.DataTables.Dto;
using BiteBliss.Models.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BiteBliss.Web.Controllers;

[Route("order")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApiResponse _response;

    public OrderController(IUnitOfWork unitOfWork)
    {
        _response = new ApiResponse();
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<ActionResult> GetOrder(string? userId)
    {
        try
        {
            var order = await _unitOfWork.Order.GetOrderAsync(userId);

            if (order == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Order not found");
                _response.IsSuccess = false;

                return BadRequest(_response);
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = order;

            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.ErrorMessages.Add(ex.Message);
            _response.IsSuccess = false;

            return BadRequest(_response);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetOrder(int id)
    {
        try
        {
            if (id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Order Id is null or empty");
                _response.IsSuccess = false;

                return BadRequest(_response);
            }

            var orderDetails = await _unitOfWork.Order.GetOrderAsync(id);

            if (orderDetails == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Order not found");
                _response.IsSuccess = false;

                return BadRequest(_response);
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = orderDetails;

            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.ErrorMessages.Add(ex.Message);
            _response.IsSuccess = false;

            return BadRequest(_response);
        }
    }

    [HttpPost("create-order")]
    public async Task<ActionResult> CreateOrder([FromBody]OrderHeaderCreateDTO orderHeaderDTO)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = ModelState.Values.SelectMany(u => u.Errors.Select(u => u.ErrorMessage)).ToList();
                _response.IsSuccess = false;

                return BadRequest(_response);
            }

            var createdOrder = await _unitOfWork.Order.CreateOrderAsync(orderHeaderDTO);

            if (createdOrder == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Order creation failed");
                _response.IsSuccess = false;

                return BadRequest(_response);
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = createdOrder;

            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.ErrorMessages.Add(ex.Message);
            _response.IsSuccess = false;

            return BadRequest(_response);
        }
    }

    [HttpPut("update-order/{id:int}")]
    public async Task<ActionResult> UpdateOrder(int id, [FromBody]OrderHeaderUpdateDTO orderHeaderUpdateDTO)
    {
        try
        {
            if (!ModelState.IsValid || id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = ModelState.Values.SelectMany(u => u.Errors.Select(u => u.ErrorMessage)).ToList();
                _response.IsSuccess = false;

                return BadRequest(_response);
            }

            var updatedOrder = await _unitOfWork.Order.UpdateOrderHeaderAsync(id, orderHeaderUpdateDTO);

            if (updatedOrder == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Order update failed");
                _response.IsSuccess = false;

                return BadRequest(_response);
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = updatedOrder;

            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.ErrorMessages.Add(ex.Message);
            _response.IsSuccess = false;

            return BadRequest(_response);
        }
    }
}
