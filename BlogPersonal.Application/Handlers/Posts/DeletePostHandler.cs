using BlogPersonal.Application.Commands.Posts;
using BlogPersonal.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BlogPersonal.Application.Handlers.Posts
{
    public class DeletePostHandler : IRequestHandler<DeletePostCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public DeletePostHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _context.Posts.FindAsync(new object[] { request.Id }, cancellationToken);

            if (post == null)
            {
                return false;
            }

            if (post.AutorId != request.UserId && !request.IsAdmin)
            {
                throw new UnauthorizedAccessException("You are not authorized to delete this post");
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
