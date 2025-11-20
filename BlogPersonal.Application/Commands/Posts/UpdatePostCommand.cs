using BlogPersonal.Application.DTOs.Posts;
using MediatR;

namespace BlogPersonal.Application.Commands.Posts
{
    public class UpdatePostCommand : IRequest<PostDto>
    {
        public int Id { get; }
        public UpdatePostDto PostDto { get; }
        public int UserId { get; }
        public bool IsAdmin { get; }

        public UpdatePostCommand(int id, UpdatePostDto postDto, int userId, bool isAdmin)
        {
            Id = id;
            PostDto = postDto;
            UserId = userId;
            IsAdmin = isAdmin;
        }
    }
}
