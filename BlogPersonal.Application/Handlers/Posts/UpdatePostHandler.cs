using AutoMapper;
using BlogPersonal.Application.Commands.Posts;
using BlogPersonal.Application.DTOs.Posts;
using BlogPersonal.Application.Interfaces;
using BlogPersonal.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlogPersonal.Application.Handlers.Posts
{
    public class UpdatePostHandler : IRequestHandler<UpdatePostCommand, PostDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdatePostHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PostDto> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _context.Posts
                .Include(p => p.PostCategorias)
                .Include(p => p.PostEtiquetas)
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (post == null)
            {
                throw new Exception("Post not found");
            }

            if (post.AutorId != request.UserId && !request.IsAdmin)
            {
                throw new UnauthorizedAccessException("You are not authorized to update this post");
            }

            var dto = request.PostDto;

            post.Titulo = dto.Titulo;
            post.Contenido = dto.Contenido;
            post.Resumen = dto.Resumen;
            post.IdiomaId = dto.IdiomaId;
            post.EstadoId = dto.EstadoId;
            post.PermitirComentarios = dto.PermitirComentarios;
            post.FechaPublicacion = dto.FechaPublicacion;
            // post.FechaActualizacion = DateTime.UtcNow; // Property does not exist

            // Update Categories
            _context.PostCategorias.RemoveRange(post.PostCategorias);
            if (dto.CategoriaIds != null && dto.CategoriaIds.Any())
            {
                foreach (var catId in dto.CategoriaIds)
                {
                    _context.PostCategorias.Add(new PostCategoria { PostId = post.Id, CategoriaId = catId });
                }
            }

            // Update Tags
            _context.PostEtiquetas.RemoveRange(post.PostEtiquetas);
            if (dto.EtiquetaIds != null && dto.EtiquetaIds.Any())
            {
                foreach (var tagId in dto.EtiquetaIds)
                {
                    _context.PostEtiquetas.Add(new PostEtiqueta { PostId = post.Id, EtiquetaId = tagId });
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            var updatedPost = await _context.Posts
                .Include(p => p.Autor)
                .Include(p => p.Estado)
                .Include(p => p.Idioma)
                .Include(p => p.PostCategorias).ThenInclude(pc => pc.Categoria)
                .Include(p => p.PostEtiquetas).ThenInclude(pe => pe.Etiqueta)
                .FirstOrDefaultAsync(p => p.Id == post.Id, cancellationToken);

            return _mapper.Map<PostDto>(updatedPost);
        }
    }
}
