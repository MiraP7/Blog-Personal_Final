using BlogPersonal.Application.DTOs.Posts;
using MediatR;
using System.Collections.Generic;

namespace BlogPersonal.Application.Queries.Posts
{
    public class GetPostsQuery : IRequest<List<PostDto>>
    {
        public string? SearchTerm { get; set; }

        public GetPostsQuery(string? searchTerm = null)
        {
            SearchTerm = searchTerm;
        }
    }
}
