using System;
using System.Collections.Generic;

namespace BlogPersonal.Domain.Entities
{
    public class Comentario
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int AutorId { get; set; }
        public string Contenido { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public int EstadoId { get; set; }
        public int? ComentarioPadreId { get; set; }

        // Navigation properties
        public Post Post { get; set; } = null!;
        public Usuario Autor { get; set; } = null!;
        public EstadoComentario Estado { get; set; } = null!;
        public Comentario? ComentarioPadre { get; set; }
        public ICollection<Comentario> Respuestas { get; set; } = new List<Comentario>();
    }
}
