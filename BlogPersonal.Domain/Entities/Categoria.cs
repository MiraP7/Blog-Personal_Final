using System.Collections.Generic;

namespace BlogPersonal.Domain.Entities
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;

        // Navigation properties
        public ICollection<PostCategoria> PostCategorias { get; set; } = new List<PostCategoria>();
    }
}
