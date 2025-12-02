using AutoMapper;
using BlogPersonal.Application.DTOs.Posts;
using BlogPersonal.Application.Interfaces;
using BlogPersonal.Application.Queries.Posts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BlogPersonal.Application.Handlers.Posts
{
    public class GetPostsHandler : IRequestHandler<GetPostsQuery, List<PostDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetPostsHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<PostDto>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Posts
                .Include(p => p.Autor)
                .Include(p => p.Estado)
                .Include(p => p.Idioma)
                .Include(p => p.Comentarios)
                .Include(p => p.PostCategorias).ThenInclude(pc => pc.Categoria)
                .Include(p => p.PostEtiquetas).ThenInclude(pe => pe.Etiqueta)
                .Where(p => p.EstadoId == 2) // Only Public posts
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                string term = request.SearchTerm.ToLower();
                query = query.Where(p => 
                    p.Titulo.ToLower().Contains(term) || 
                    p.Contenido.ToLower().Contains(term) ||
                    (p.Resumen != null && p.Resumen.ToLower().Contains(term)));
            }

            var posts = await query.ToListAsync(cancellationToken);

            return _mapper.Map<List<PostDto>>(posts);
        }
    }
}
