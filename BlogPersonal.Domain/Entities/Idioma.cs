using System.Collections.Generic;

namespace BlogPersonal.Domain.Entities
{
    public class Idioma
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty; // e.g., "es", "en"
        public string Nombre { get; set; } = string.Empty;

        // Navigation properties
        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}
