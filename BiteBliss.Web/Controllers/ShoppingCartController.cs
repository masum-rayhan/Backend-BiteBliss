using BiteBliss.DataAccess.Repo.IRepo;
using BiteBliss.Models.DataTables.Dto;
using BiteBliss.Models.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BiteBliss.Web.Controllers;

[Route("shopping-cart")]
[ApiController]
public class ShoppingCartController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApiResponse _response;
    public ShoppingCartController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _response = new ApiResponse();
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetShoppingCart(string userId)
    {
        try
        {
            if (string.IsNullOrEmpty(userId))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;

                return BadRequest(_response);
            }
            var shoppingCarts = await _unitOfWork.ShoppingCart.GetAsync(userId);

            _response.Result = shoppingCarts;
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.ToString() };

            return BadRequest(_response);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrUpdateItemInCartAsync(string userId, int menuItemId, int updateQuantityBy)
    {
        try
        {
            var shoppingCart = await _unitOfWork.ShoppingCart.CreateOrUpdateItemInCartAsync(userId, menuItemId, updateQuantityBy);
            if (shoppingCart == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;

                return BadRequest(_response);
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = shoppingCart;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.ErrorMessages = new List<string> { ex.ToString() };
            _response.IsSuccess = false;

            return BadRequest(_response);
        }        
    }
}
