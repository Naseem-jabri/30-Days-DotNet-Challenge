using AuthNest_API.DTOS;
using AuthNest_API.Model;

namespace AuthNest_API.Services
{
    public interface IUserService
    {
        Task<object> CreateUserAsync(CreateUserDto userDto);

        object Login(LoginDto loginDto);

        string ConfirmEmail(string token);

        List<User> GetUsers();
    }
}