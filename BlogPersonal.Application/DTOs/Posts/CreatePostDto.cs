using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogPersonal.Application.DTOs.Posts
{
    public class CreatePostDto
    {
        [Required]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        public string Contenido { get; set; } = string.Empty;

        public string? Resumen { get; set; }

        [Required]
        public int IdiomaId { get; set; }

        public int EstadoId { get; set; }

        public bool PermitirComentarios { get; set; } = true;

        public DateTime? FechaPublicacion { get; set; }

        public List<int> CategoriaIds { get; set; } = new List<int>();
        public List<int> EtiquetaIds { get; set; } = new List<int>();
    }
}
