using AutoMapper;
using BlogPersonal.Application.Commands.Posts;
using BlogPersonal.Application.DTOs.Posts;
using BlogPersonal.Application.Interfaces;
using BlogPersonal.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace BlogPersonal.Application.Handlers.Posts
{
    public class CreatePostHandler : IRequestHandler<CreatePostCommand, PostDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreatePostHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PostDto> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var dto = request.PostDto;

            // Generate Slug - URL-safe
            var slug = GenerateSlug(dto.Titulo);
            if (await _context.Posts.AnyAsync(p => p.Slug == slug, cancellationToken))
            {
                slug = $"{slug}-{Guid.NewGuid().ToString().Substring(0, 8)}";
            }

            var post = new Post
            {
                Titulo = dto.Titulo,
                Slug = slug,
                Contenido = dto.Contenido,
                Resumen = dto.Resumen,
                AutorId = request.AutorId,
                EstadoId = dto.EstadoId > 0 ? dto.EstadoId : 1, // Use provided ID or default to Borrador
                IdiomaId = dto.IdiomaId,
                PermitirComentarios = dto.PermitirComentarios,
                FechaCreacion = DateTime.UtcNow,
                FechaPublicacion = dto.FechaPublicacion
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync(cancellationToken);

            // Add Categories
            if (dto.CategoriaIds != null && dto.CategoriaIds.Any())
            {
                foreach (var catId in dto.CategoriaIds)
                {
                    _context.PostCategorias.Add(new PostCategoria { PostId = post.Id, CategoriaId = catId });
                }
            }

            // Add Tags
            if (dto.EtiquetaIds != null && dto.EtiquetaIds.Any())
            {
                foreach (var tagId in dto.EtiquetaIds)
                {
                    _context.PostEtiquetas.Add(new PostEtiqueta { PostId = post.Id, EtiquetaId = tagId });
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            // Reload for mapping
            var createdPost = await _context.Posts
                .Include(p => p.Autor)
                .Include(p => p.Estado)
                .Include(p => p.Idioma)
                .Include(p => p.PostCategorias).ThenInclude(pc => pc.Categoria)
                .Include(p => p.PostEtiquetas).ThenInclude(pe => pe.Etiqueta)
                .FirstOrDefaultAsync(p => p.Id == post.Id, cancellationToken);

            return _mapper.Map<PostDto>(createdPost);
        }

        private static string GenerateSlug(string title)
        {
            // Normalize and remove diacritics (á -> a, ñ -> n, etc.)
            var normalizedString = title.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            var slug = stringBuilder.ToString().Normalize(NormalizationForm.FormC);
            
            // Convert to lowercase
            slug = slug.ToLowerInvariant();
            
            // Replace spaces with hyphens
            slug = slug.Replace(" ", "-");
            
            // Remove all non-alphanumeric characters except hyphens
            slug = Regex.Replace(slug, @"[^a-z0-9\-]", "");
            
            // Replace multiple hyphens with single hyphen
            slug = Regex.Replace(slug, @"-+", "-");
            
            // Trim hyphens from start and end
            slug = slug.Trim('-');

            return slug;
        }
    }
}
