using BlogPersonal.Application.DTOs.Posts;
using MediatR;

namespace BlogPersonal.Application.Queries.Posts
{
    public class GetPostByIdQuery : IRequest<PostDto>
    {
        public int Id { get; set; }

        public GetPostByIdQuery(int id)
        {
            Id = id;
        }
    }
}
