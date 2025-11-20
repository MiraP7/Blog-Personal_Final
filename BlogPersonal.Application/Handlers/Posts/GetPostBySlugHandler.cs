using AutoMapper;
using BlogPersonal.Application.DTOs.Posts;
using BlogPersonal.Application.Interfaces;
using BlogPersonal.Application.Queries.Posts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace BlogPersonal.Application.Handlers.Posts
{
    public class GetPostBySlugHandler : IRequestHandler<GetPostBySlugQuery, PostDto?>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetPostBySlugHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PostDto?> Handle(GetPostBySlugQuery request, CancellationToken cancellationToken)
        {
            var post = await _context.Posts
                .Include(p => p.Autor)
                .Include(p => p.Estado)
                .Include(p => p.Idioma)
                .Include(p => p.PostCategorias).ThenInclude(pc => pc.Categoria)
                .Include(p => p.PostEtiquetas).ThenInclude(pe => pe.Etiqueta)
                .FirstOrDefaultAsync(p => p.Slug == request.Slug, cancellationToken);

            return _mapper.Map<PostDto>(post);
        }
    }
}
