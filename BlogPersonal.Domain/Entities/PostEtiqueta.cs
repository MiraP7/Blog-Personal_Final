namespace BlogPersonal.Domain.Entities
{
    public class PostEtiqueta
    {
        public int PostId { get; set; }
        public int EtiquetaId { get; set; }

        // Navigation properties
        public Post Post { get; set; } = null!;
        public Etiqueta Etiqueta { get; set; } = null!;
    }
}
