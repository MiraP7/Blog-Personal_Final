using System.Collections.Generic;

namespace BlogPersonal.Domain.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public int RolId { get; set; }
        public bool Activo { get; set; } = true;

        // Navigation properties
        public Rol Rol { get; set; } = null!;
        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();
    }
}
