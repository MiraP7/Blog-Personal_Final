using BlogPersonal.Application.DTOs.Auth;
using System.Threading.Tasks;

namespace BlogPersonal.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterUserDto registerDto);
        Task<AuthResponseDto> LoginAsync(LoginUserDto loginDto);
    }
}
