using BlogPersonal.Application.DTOs.Comments;
using MediatR;
using System.Collections.Generic;

namespace BlogPersonal.Application.Queries.Comments
{
    public record GetCommentsByPostIdQuery(int PostId) : IRequest<List<CommentDto>>;
}
