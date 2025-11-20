using System;
using System.Collections.Generic;

namespace BlogPersonal.Application.DTOs.Posts
{
    public class PostDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Contenido { get; set; } = string.Empty;
        public string? Resumen { get; set; }
        public string AutorNombre { get; set; } = string.Empty;
        public int AutorId { get; set; }
        public string Estado { get; set; } = string.Empty;
        public int EstadoId { get; set; }
        public string Idioma { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public int Vistas { get; set; }
        public List<string> Categorias { get; set; } = new List<string>();
        public List<EtiquetaDto> Etiquetas { get; set; } = new List<EtiquetaDto>();
    }
}
