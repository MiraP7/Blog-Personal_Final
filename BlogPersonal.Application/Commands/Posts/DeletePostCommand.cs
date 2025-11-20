using MediatR;

namespace BlogPersonal.Application.Commands.Posts
{
    public class DeletePostCommand : IRequest<bool>
    {
        public int Id { get; }
        public int UserId { get; }
        public bool IsAdmin { get; }

        public DeletePostCommand(int id, int userId, bool isAdmin)
        {
            Id = id;
            UserId = userId;
            IsAdmin = isAdmin;
        }
    }
}
