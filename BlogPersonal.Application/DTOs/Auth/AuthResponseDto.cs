namespace BlogPersonal.Application.DTOs.Auth
{
    public class AuthResponseDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
    }
}
