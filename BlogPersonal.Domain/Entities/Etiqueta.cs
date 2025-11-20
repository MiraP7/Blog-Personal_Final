using System.Collections.Generic;

namespace BlogPersonal.Domain.Entities
{
    public class Etiqueta
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;

        // Navigation properties
        public ICollection<PostEtiqueta> PostEtiquetas { get; set; } = new List<PostEtiqueta>();
    }
}
