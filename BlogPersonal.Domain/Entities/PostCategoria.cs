namespace BlogPersonal.Domain.Entities
{
    public class PostCategoria
    {
        public int PostId { get; set; }
        public int CategoriaId { get; set; }

        // Navigation properties
        public Post Post { get; set; } = null!;
        public Categoria Categoria { get; set; } = null!;
    }
}
