using BlogPersonal.Application.DTOs.Comments;
using MediatR;

namespace BlogPersonal.Application.Commands.Comments
{
    public record CreateCommentCommand(CreateCommentDto CommentDto, int UserId) : IRequest<CommentDto>;
}
