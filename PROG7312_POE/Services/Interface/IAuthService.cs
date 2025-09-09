using PROG7312_POE.Models;

namespace PROG7312_POE.Services.Interface
{
    public interface IAuthService
    {
        Task<(bool Success, string ErrorMessage)> RegisterUserAsync(userTBL user);
        Task<userTBL?> AuthenticateUserAsync(string email, string password);
    }
}
