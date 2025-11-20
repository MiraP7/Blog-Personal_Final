using AutoMapper;
using BlogPersonal.Application.DTOs.Posts;
using BlogPersonal.Application.Interfaces;
using BlogPersonal.Application.Queries.Posts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlogPersonal.Application.Handlers.Posts
{
    public class GetMyPostsHandler : IRequestHandler<GetMyPostsQuery, List<PostDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetMyPostsHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<PostDto>> Handle(GetMyPostsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Posts
                .Include(p => p.Autor)
                .Include(p => p.Estado)
                .Include(p => p.Idioma)
                .Include(p => p.PostCategorias).ThenInclude(pc => pc.Categoria)
                .Include(p => p.PostEtiquetas).ThenInclude(pe => pe.Etiqueta)
                .Where(p => p.AutorId == request.UserId)
                .AsQueryable();

            if (request.EstadoId.HasValue)
            {
                query = query.Where(p => p.EstadoId == request.EstadoId.Value);
            }

            var posts = await query.OrderByDescending(p => p.FechaPublicacion).ToListAsync(cancellationToken);

            return _mapper.Map<List<PostDto>>(posts);
        }
    }
}
