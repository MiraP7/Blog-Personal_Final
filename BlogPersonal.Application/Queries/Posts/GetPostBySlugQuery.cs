using BlogPersonal.Application.DTOs.Posts;
using MediatR;

namespace BlogPersonal.Application.Queries.Posts
{
    public record GetPostBySlugQuery(string Slug) : IRequest<PostDto?>;
}
