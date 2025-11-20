using AutoMapper;
using BlogPersonal.Application.DTOs.Comments;
using BlogPersonal.Application.Interfaces;
using BlogPersonal.Application.Queries.Comments;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlogPersonal.Application.Handlers.Comments
{
    public class GetCommentsByPostIdHandler : IRequestHandler<GetCommentsByPostIdQuery, List<CommentDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCommentsByPostIdHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CommentDto>> Handle(GetCommentsByPostIdQuery request, CancellationToken cancellationToken)
        {
            var comments = await _context.Comentarios
                .Include(c => c.Autor)
                .Where(c => c.PostId == request.PostId)
                .OrderBy(c => c.FechaCreacion)
                .ToListAsync(cancellationToken);

            var commentDtos = _mapper.Map<List<CommentDto>>(comments);

            var rootComments = commentDtos.Where(c => c.ComentarioPadreId == null).ToList();
            foreach (var root in rootComments)
            {
                AddChildren(root, commentDtos);
            }

            return rootComments;
        }

        private void AddChildren(CommentDto parent, List<CommentDto> allComments)
        {
            parent.Respuestas = allComments.Where(c => c.ComentarioPadreId == parent.Id).ToList();
            foreach (var child in parent.Respuestas)
            {
                AddChildren(child, allComments);
            }
        }
    }
}
