using BiteBliss.DataAccess.Repo.IRepo;
using BiteBliss.DataAccess.Repo.IRepo.Services;
using BiteBliss.Models.DataTables.Dto;
using BiteBliss.Models.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BiteBliss.Web.Controllers;

[Route("menuItem")]
[ApiController]
public class MenuItemController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private ApiResponse _response;
    private readonly ICacheService _cacheService;

    public MenuItemController(IUnitOfWork unitOfWork, ICacheService cacheService)
    {
        _unitOfWork = unitOfWork;
        _response = new ApiResponse();
        _cacheService = cacheService;
    }

    [HttpGet]
    public async Task<IActionResult> GetMenuItems()
    {
        _response.Result = await _unitOfWork.MenuItems.GetAllAsync();
        _response.StatusCode = HttpStatusCode.OK;

        return Ok(_response);
    }

    [HttpGet("{id:int}", Name = "GetMenuItems")]
    public async Task<IActionResult> GetMenuItems(int id)
    {
        if (id == 0)
        {
            _response.StatusCode = HttpStatusCode.BadRequest;
            return BadRequest(_response);
        }

        var item = await _unitOfWork.MenuItems.GetDetailsAsync(id);

        if (item == null)
        {
            _response.StatusCode = HttpStatusCode.NotFound;
            return NotFound(_response);
        }

        _response.Result = item;
        _response.StatusCode = HttpStatusCode.OK;

        return Ok(_response);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse>> CreateMenuItem([FromForm] MenuItemCreateDTO menuItemCreateDTO)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var createdMenuItem = await _unitOfWork.MenuItems.CreateMenuItemAsync(menuItemCreateDTO);

                if (createdMenuItem == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                if (createdMenuItem != null)
                {
                    _response.Result = createdMenuItem;
                    _response.StatusCode = HttpStatusCode.Created;

                    return CreatedAtRoute("GetMenuItems", new { id = createdMenuItem.Id }, _response);
                }
            }
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.ToString() };
        }

        return Ok(_response);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse>> UpdateMenuItem(int id, [FromForm] MenuItemUpdateDTO menuItemUpdateDTO)
    {
        try
        {
            if(ModelState.IsValid)
            {
                var updatedMenuItem = await _unitOfWork.MenuItems.UpdateMenuItemAsync(id, menuItemUpdateDTO);

                if (updatedMenuItem == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                if (updatedMenuItem != null)
                {
                    _response.Result = updatedMenuItem;
                    _response.StatusCode = HttpStatusCode.OK;

                    return Ok(_response);
                }
            }
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.ToString() };
        }
        return Ok(_response);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse>> DeleteMenuItem(int id)
    {
        try
        {
            if (id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var deletedMenuItem = await _unitOfWork.MenuItems.DeleteMenuItemAsync(id);

            int milliseconds = 1000;
            Thread.Sleep(milliseconds);

            if (deletedMenuItem == false)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            if (deletedMenuItem == true)
            {
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.ToString() };
        }

        return Ok(_response);
    }   
}
