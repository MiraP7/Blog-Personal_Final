using BlogPersonal.Application.Interfaces;
using BlogPersonal.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogPersonal.Infrastructure.Data
{
    public class BlogDbContext : DbContext, IApplicationDbContext
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Etiqueta> Etiquetas { get; set; }
        public DbSet<PostCategoria> PostCategorias { get; set; }
        public DbSet<PostEtiqueta> PostEtiquetas { get; set; }
        public DbSet<EstadoPost> EstadosPost { get; set; }
        public DbSet<EstadoComentario> EstadosComentario { get; set; }
        public DbSet<Idioma> Idiomas { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships and constraints

            // Usuario
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();
            
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Rol)
                .WithMany(r => r.Usuarios)
                .HasForeignKey(u => u.RolId);

            // Post
            modelBuilder.Entity<Post>()
                .HasIndex(p => p.Slug)
                .IsUnique();

            modelBuilder.Entity<Post>()
                .HasOne(p => p.Autor)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.AutorId);

            modelBuilder.Entity<Post>()
                .HasOne(p => p.Estado)
                .WithMany(e => e.Posts)
                .HasForeignKey(p => p.EstadoId);

            modelBuilder.Entity<Post>()
                .HasOne(p => p.Idioma)
                .WithMany(i => i.Posts)
                .HasForeignKey(p => p.IdiomaId);

            // Comentario
            modelBuilder.Entity<Comentario>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comentarios)
                .HasForeignKey(c => c.PostId);

            modelBuilder.Entity<Comentario>()
                .HasOne(c => c.Autor)
                .WithMany(u => u.Comentarios)
                .HasForeignKey(c => c.AutorId);

            modelBuilder.Entity<Comentario>()
                .HasOne(c => c.Estado)
                .WithMany(e => e.Comentarios)
                .HasForeignKey(c => c.EstadoId);

            modelBuilder.Entity<Comentario>()
                .HasOne(c => c.ComentarioPadre)
                .WithMany(c => c.Respuestas)
                .HasForeignKey(c => c.ComentarioPadreId);

            // Many-to-Many: PostCategoria
            modelBuilder.Entity<PostCategoria>()
                .HasKey(pc => new { pc.PostId, pc.CategoriaId });

            modelBuilder.Entity<PostCategoria>()
                .HasOne(pc => pc.Post)
                .WithMany(p => p.PostCategorias)
                .HasForeignKey(pc => pc.PostId);

            modelBuilder.Entity<PostCategoria>()
                .HasOne(pc => pc.Categoria)
                .WithMany(c => c.PostCategorias)
                .HasForeignKey(pc => pc.CategoriaId);

            // Many-to-Many: PostEtiqueta
            modelBuilder.Entity<PostEtiqueta>()
                .HasKey(pe => new { pe.PostId, pe.EtiquetaId });

            modelBuilder.Entity<PostEtiqueta>()
                .HasOne(pe => pe.Post)
                .WithMany(p => p.PostEtiquetas)
                .HasForeignKey(pe => pe.PostId);

            modelBuilder.Entity<PostEtiqueta>()
                .HasOne(pe => pe.Etiqueta)
                .WithMany(e => e.PostEtiquetas)
                .HasForeignKey(pe => pe.EtiquetaId);

            // Seed Data
            modelBuilder.Entity<Rol>().HasData(
                new Rol { Id = 1, Nombre = "Administrador", Descripcion = "Acceso total al sistema" },
                new Rol { Id = 2, Nombre = "Autor", Descripcion = "Puede crear y gestionar sus propios posts" },
                new Rol { Id = 3, Nombre = "Usuario", Descripcion = "Puede comentar" },
                new Rol { Id = 4, Nombre = "Visitante", Descripcion = "Solo lectura" }
            );

            modelBuilder.Entity<EstadoPost>().HasData(
                new EstadoPost { Id = 1, Nombre = "Borrador" },
                new EstadoPost { Id = 2, Nombre = "Publicado" },
                new EstadoPost { Id = 3, Nombre = "Archivado" },
                new EstadoPost { Id = 4, Nombre = "Privado" }
            );

            modelBuilder.Entity<EstadoComentario>().HasData(
                new EstadoComentario { Id = 1, Nombre = "Pendiente" },
                new EstadoComentario { Id = 2, Nombre = "Aprobado" },
                new EstadoComentario { Id = 3, Nombre = "Rechazado" }
            );

            modelBuilder.Entity<Idioma>().HasData(
                new Idioma { Id = 1, Codigo = "es", Nombre = "Español" },
                new Idioma { Id = 2, Codigo = "en", Nombre = "Inglés" }
            );
        }
    }
}
