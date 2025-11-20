using BlogPersonal.Application.DTOs.Posts;
using MediatR;

namespace BlogPersonal.Application.Commands.Posts
{
    public class CreatePostCommand : IRequest<PostDto>
    {
        public CreatePostDto PostDto { get; set; }
        public int AutorId { get; set; }

        public CreatePostCommand(CreatePostDto postDto, int autorId)
        {
            PostDto = postDto;
            AutorId = autorId;
        }
    }
}
