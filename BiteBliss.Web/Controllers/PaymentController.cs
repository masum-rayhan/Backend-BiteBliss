using BiteBliss.DataAccess.Repo.IRepo;
using BiteBliss.Models.DataTables.Dto;
using BiteBliss.Models.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BiteBliss.Web.Controllers;

[Route("payment")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApiResponse _response;

    public PaymentController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _response = new ApiResponse();
    }

    [HttpGet]
    public async Task<IActionResult> CreatePayment(string userId)
    {
        var result = await _unitOfWork.Payments.CreatePaymentAsync(userId);

        if (result == null)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.ErrorMessages.Add("Something went wrong while creating payment");
            _response.IsSuccess = false;

            return BadRequest(_response);
        }

        _response.StatusCode = HttpStatusCode.OK;
        _response.IsSuccess = true;
        _response.Result = result;

        return Ok(_response);
    }
}
