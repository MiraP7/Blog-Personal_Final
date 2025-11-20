using System.Collections.Generic;

namespace BlogPersonal.Domain.Entities
{
    public class EstadoPost
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;

        // Navigation properties
        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}
