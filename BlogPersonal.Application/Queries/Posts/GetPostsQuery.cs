using BlogPersonal.Application.DTOs.Posts;
using MediatR;
using System.Collections.Generic;

namespace BlogPersonal.Application.Queries.Posts
{
    public class GetPostsQuery : IRequest<List<PostDto>>
    {
        public string? SearchTerm { get; set; }
        public int? UserId { get; set; }
        public string? UserRole { get; set; }

        public GetPostsQuery(string? searchTerm = null, int? userId = null, string? userRole = null)
        {
            SearchTerm = searchTerm;
            UserId = userId;
            UserRole = userRole;
        }
    }
}
