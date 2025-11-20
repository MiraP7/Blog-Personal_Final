using BlogPersonal.Domain.Entities;

namespace BlogPersonal.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(Usuario usuario);
    }
}
