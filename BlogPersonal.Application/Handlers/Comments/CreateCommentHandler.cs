using AutoMapper;
using BlogPersonal.Application.Commands.Comments;
using BlogPersonal.Application.DTOs.Comments;
using BlogPersonal.Application.Interfaces;
using BlogPersonal.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BlogPersonal.Application.Handlers.Comments
{
    public class CreateCommentHandler : IRequestHandler<CreateCommentCommand, CommentDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateCommentHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CommentDto> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var post = await _context.Posts.FindAsync(new object[] { request.CommentDto.PostId }, cancellationToken);
            if (post == null) throw new Exception("Post no encontrado");

            var estadoPendiente = await _context.EstadosComentario.FirstOrDefaultAsync(e => e.Nombre == "Pendiente", cancellationToken);
            if (estadoPendiente == null) 
            {
                 // Fallback if seed didn't run or name is different
                 estadoPendiente = await _context.EstadosComentario.FirstOrDefaultAsync(cancellationToken);
            }

            var comment = new Comentario
            {
                PostId = request.CommentDto.PostId,
                AutorId = request.UserId,
                Contenido = request.CommentDto.Contenido,
                ComentarioPadreId = request.CommentDto.ComentarioPadreId,
                EstadoId = estadoPendiente!.Id,
                FechaCreacion = DateTime.UtcNow
            };

            _context.Comentarios.Add(comment);
            await _context.SaveChangesAsync(cancellationToken);

            var createdComment = await _context.Comentarios
                .Include(c => c.Autor)
                .FirstOrDefaultAsync(c => c.Id == comment.Id, cancellationToken);

            return _mapper.Map<CommentDto>(createdComment);
        }
    }
}
