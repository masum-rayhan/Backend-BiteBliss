using BiteBliss.DataAcces.Data;
using BiteBliss.DataAcces.Repo.IRepo.Auth;
using BiteBliss.DataAcces.Utils;
using BiteBliss.Models;
using BiteBliss.Models.Auth.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BiteBliss.DataAcces.Repo.Auth;

public class AuthRepo : IAuthRepo
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;
    private string secretKey;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AuthRepo(AppDbContext db, IConfiguration config, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        _db = db;
        secretKey = config.GetValue<string>("ApiSettings:Secret");
        _roleManager = roleManager;
        _userManager = userManager;
    }
    public async Task<ApplicationUser> RegisterUserAsync(RegisterRequestDTO user)
    {
        ApplicationUser userFromDb = _db.Users.FirstOrDefault(u => u.UserName.ToLower() == user.UserName.ToLower());

        if (userFromDb != null)
            throw new ArgumentException("User already exists");

        ApplicationUser newUser = new()
        {
            UserName = user.UserName,
            Email = user.Email,
            NormalizedEmail = user.UserName.ToUpper(),
            Name = user.Name,
        };


        var result = await _userManager.CreateAsync(newUser, user.Password);

        try
        {
            if (result.Succeeded)
            {
                if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
                {
                    await _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin));
                    await _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer));
                }
                //if(!_roleManager.RoleExistsAsync(SD.Role_Customer).GetAwaiter().GetResult())
                //{
                //    await _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer));
                //}
                if (user.Role.ToLower() == SD.Role_Admin)
                {
                    await _userManager.AddToRoleAsync(newUser, SD.Role_Admin);
                }
                else
                {
                    await _userManager.AddToRoleAsync(newUser, SD.Role_Customer);
                }
            }
            else
            {
                // Registration failed, handle the error
                string errorDescription = string.Join(", ", result.Errors.Select(e => e.Description));
                var errorMessage = $"User registration failed: {errorDescription}";
                throw new ArgumentException(errorMessage);
            }
        }
        catch (Exception ex)
        {
            throw new ArgumentException(ex.Message);
        }
       
        return newUser;
    }
    public async Task<LoginResponseDTO> LoginAsync(LoginRequestDTO user)
    {
        ApplicationUser userFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == user.UserName.ToLower());

        if (userFromDb == null)
            throw new ArgumentException("User does not exist");

        bool isValid = await _userManager.CheckPasswordAsync(userFromDb, user.Password);

        if (isValid == false)
            throw new ArgumentException("Invalid credentials");

        JwtSecurityTokenHandler tokenHandler = new();
        byte[] key = Encoding.ASCII.GetBytes(secretKey);

        var roles = await _userManager.GetRolesAsync(userFromDb);

        SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromDb.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromDb.Name),
                new Claim(ClaimTypes.Email, userFromDb.UserName.ToString()),
                new Claim(ClaimTypes.Role, roles.FirstOrDefault())
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(securityTokenDescriptor);

        LoginResponseDTO response = new()
        {
            Email = userFromDb.Email,
            Token = tokenHandler.WriteToken(token)
        };

        return response;
    }

    public Task<bool> ResetPasswordAsync(ApplicationUser user, string newPassword)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UserExistsAsync(string username)
    {
        throw new NotImplementedException();
    }

    //public async Task<string> GenerateJwtToken(LoginRequestDTO user)
    //{
    //    ApplicationUser userFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == user.UserName.ToLower());

    //    JwtSecurityTokenHandler tokenHandler = new();
    //    byte[] key = Encoding.ASCII.GetBytes(secretKey);

    //    var roles = await _userManager.GetRolesAsync(userFromDb);

    //    SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
    //    {
    //        Subject = new ClaimsIdentity(new Claim[]
    //        {
    //            new Claim(ClaimTypes.NameIdentifier, userFromDb.Id.ToString()),
    //            new Claim(ClaimTypes.Name, userFromDb.Name),
    //            new Claim(ClaimTypes.Email, userFromDb.UserName.ToString()),
    //            new Claim(ClaimTypes.Role, roles.FirstOrDefault())
    //        }),
    //        Expires = DateTime.UtcNow.AddDays(7),
    //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    //    };

    //    var token = tokenHandler.CreateToken(securityTokenDescriptor);

    //    LoginResponseDTO response = new()
    //    {
    //        Email = userFromDb.Email,
    //        Token = tokenHandler.WriteToken(token)
    //    };

    //    //return tokenHandler.WriteToken(token);
    //}
}
