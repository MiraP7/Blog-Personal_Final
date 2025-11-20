using BlogPersonal.Application.DTOs.Posts;
using MediatR;
using System.Collections.Generic;

namespace BlogPersonal.Application.Queries.Posts
{
    public class GetMyPostsQuery : IRequest<List<PostDto>>
    {
        public int UserId { get; set; }
        public int? EstadoId { get; set; }

        public GetMyPostsQuery(int userId, int? estadoId = null)
        {
            UserId = userId;
            EstadoId = estadoId;
        }
    }
}
