using System.Collections.Generic;

namespace BlogPersonal.Domain.Entities
{
    public class EstadoComentario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;

        // Navigation properties
        public ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();
    }
}
