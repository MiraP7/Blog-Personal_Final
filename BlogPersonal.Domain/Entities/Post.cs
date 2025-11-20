using System;
using System.Collections.Generic;

namespace BlogPersonal.Domain.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Contenido { get; set; } = string.Empty;
        public string? Resumen { get; set; }
        public int AutorId { get; set; }
        public int EstadoId { get; set; }
        public int IdiomaId { get; set; }
        public bool PermitirComentarios { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaPublicacion { get; set; }
        public int Vistas { get; set; }

        // Navigation properties
        public Usuario Autor { get; set; } = null!;
        public EstadoPost Estado { get; set; } = null!;
        public Idioma Idioma { get; set; } = null!;
        public ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();
        public ICollection<PostCategoria> PostCategorias { get; set; } = new List<PostCategoria>();
        public ICollection<PostEtiqueta> PostEtiquetas { get; set; } = new List<PostEtiqueta>();
    }
}
