using BiteBliss.DataAccess.Repo.IRepo;
using BiteBliss.Models.Auth.DTO;
using BiteBliss.Models.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Reflection.Metadata.Ecma335;

namespace BiteBliss.Web.Controllers.Auth;

[Route("auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApiResponse _response;
    public AuthController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _response = new ApiResponse();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody]RegisterRequestDTO user)
    {
        try
        {
            if (string.IsNullOrEmpty(user.Role))
            {
                // Handle the case where user.Role is missing or empty
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("User role is required.");

                return BadRequest(_response);
            }
            var newUser = await _unitOfWork.Auth.RegisterUserAsync(user);

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = "User Register Successfully";

            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorMessages.Add( ex.Message );

            return BadRequest( _response);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody]LoginRequestDTO user)
    {
        try
        {
            var userFromDb = await _unitOfWork.Auth.LoginAsync(user);

            if (userFromDb == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("User not found.");

                return NotFound(_response);
            }

            var token = await _unitOfWork.Auth.LoginAsync(user);

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = token;

            return Ok(_response);

        }
        catch (Exception ex)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            _response.ErrorMessages.Add(ex.Message);

            return BadRequest(_response);
        }
    }
}
