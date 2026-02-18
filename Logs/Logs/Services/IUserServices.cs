
using Logs.DTO;
using Logs.Models;

namespace Logs.Services
{
    public interface IUserServices
    {
        Task<(string message, bool success)> CreateUserAsync(UserRegisterDto dto);

        Task<(UsersCredential? user, string message, bool success)> LoginUserAsync(UserLoginDto dto);

        Task<(List<UsersCredential> users, string message, bool success)> GetAllUsersAsync();
    }
}
