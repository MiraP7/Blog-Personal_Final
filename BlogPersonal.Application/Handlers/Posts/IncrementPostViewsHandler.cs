using BlogPersonal.Application.Commands.Posts;
using BlogPersonal.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace BlogPersonal.Application.Handlers.Posts
{
    public class IncrementPostViewsHandler : IRequestHandler<IncrementPostViewsCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public IncrementPostViewsHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(IncrementPostViewsCommand request, CancellationToken cancellationToken)
        {
            var post = await _context.Posts
                .FirstOrDefaultAsync(p => p.Id == request.PostId, cancellationToken);

            if (post == null) return false;

            post.Vistas++;
            _context.Posts.Update(post);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
