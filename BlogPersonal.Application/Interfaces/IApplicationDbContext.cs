using BlogPersonal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace BlogPersonal.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Usuario> Usuarios { get; }
        DbSet<Rol> Roles { get; }
        DbSet<Post> Posts { get; }
        DbSet<Comentario> Comentarios { get; }
        DbSet<Categoria> Categorias { get; }
        DbSet<Etiqueta> Etiquetas { get; }
        DbSet<PostCategoria> PostCategorias { get; }
        DbSet<PostEtiqueta> PostEtiquetas { get; }
        DbSet<EstadoPost> EstadosPost { get; }
        DbSet<EstadoComentario> EstadosComentario { get; }
        DbSet<Idioma> Idiomas { get; }
        DbSet<AuditLog> AuditLogs { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
