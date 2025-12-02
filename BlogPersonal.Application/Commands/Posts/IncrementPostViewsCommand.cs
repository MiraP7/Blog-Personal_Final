using MediatR;

namespace BlogPersonal.Application.Commands.Posts
{
    public class IncrementPostViewsCommand : IRequest<bool>
    {
        public int PostId { get; set; }

        public IncrementPostViewsCommand(int postId)
        {
            PostId = postId;
        }
    }
}
