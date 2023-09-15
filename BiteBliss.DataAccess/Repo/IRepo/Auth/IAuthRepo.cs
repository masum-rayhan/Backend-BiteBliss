using BiteBliss.Models;
using BiteBliss.Models.Auth.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiteBliss.DataAcces.Repo.IRepo.Auth;

public interface IAuthRepo
{
    Task<ApplicationUser> RegisterUserAsync(RegisterRequestDTO user);
    Task<LoginResponseDTO> LoginAsync(LoginRequestDTO user);
    Task<bool> UserExistsAsync(string username);
    Task <bool> ResetPasswordAsync(ApplicationUser user, string newPassword);
    //Task<string> GenerateJwtToken(LoginRequestDTO user);
}
