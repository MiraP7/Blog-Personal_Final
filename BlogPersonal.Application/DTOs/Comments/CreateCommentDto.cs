using System.ComponentModel.DataAnnotations;

namespace BlogPersonal.Application.DTOs.Comments
{
    public class CreateCommentDto
    {
        [Required]
        public int PostId { get; set; }
        
        [Required]
        public string Contenido { get; set; } = string.Empty;
        
        public int? ComentarioPadreId { get; set; }
    }
}
