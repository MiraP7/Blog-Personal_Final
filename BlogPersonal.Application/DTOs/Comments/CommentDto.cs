using System;
using System.Collections.Generic;

namespace BlogPersonal.Application.DTOs.Comments
{
    public class CommentDto
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string AutorNombre { get; set; } = string.Empty;
        public string Contenido { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public int? ComentarioPadreId { get; set; }
        public List<CommentDto> Respuestas { get; set; } = new();
    }
}
