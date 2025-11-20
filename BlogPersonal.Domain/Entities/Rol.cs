using System.Collections.Generic;

namespace BlogPersonal.Domain.Entities
{
    public class Rol
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }

        // Navigation properties
        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
