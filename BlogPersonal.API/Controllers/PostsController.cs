using BlogPersonal.Application.Commands.Posts;
using BlogPersonal.Application.DTOs.Posts;
using BlogPersonal.Application.Queries.Posts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlogPersonal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PostsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<PostDto>>> GetPosts([FromQuery] string? search)
        {
            // Get user info if authenticated
            int? userId = null;
            string? userRole = null;
            
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null)
            {
                userId = int.Parse(userIdClaim.Value);
                userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            }

            var posts = await _mediator.Send(new GetPostsQuery(search, userId, userRole));
            return Ok(posts);
        }

        [HttpGet("my-posts")]
        [Authorize]
        public async Task<ActionResult<List<PostDto>>> GetMyPosts([FromQuery] int? estadoId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);
            var posts = await _mediator.Send(new GetMyPostsQuery(userId, estadoId));
            return Ok(posts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PostDto>> GetPost(int id)
        {
            var post = await _mediator.Send(new GetPostByIdQuery(id));
            if (post == null) return NotFound();

            // Visibility Logic based on user role:
            // EstadoId: 1=Borrador, 2=Publicado, 3=Archivado, 4=Privado
            
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            // Autor/Admin can see ALL posts
            if (userRole == "Autor" || userRole == "Administrador")
            {
                return Ok(post);
            }

            // Any authenticated user can see Public (2) + Private (4)
            if (userIdClaim != null)
            {
                if (post.EstadoId == 2 || post.EstadoId == 4)
                {
                    return Ok(post);
                }
                return Forbid();
            }

            // Anonymous users can only see Public (2)
            if (post.EstadoId == 2)
            {
                return Ok(post);
            }

            return Forbid();
        }

        [HttpGet("slug/{slug}")]
        public async Task<ActionResult<PostDto>> GetPostBySlug(string slug)
        {
            var post = await _mediator.Send(new GetPostBySlugQuery(slug));
            if (post == null) return NotFound();

            // Visibility Logic based on user role:
            // EstadoId: 1=Borrador, 2=Publicado, 3=Archivado, 4=Privado
            
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            // Autor/Admin can see ALL posts
            if (userRole == "Autor" || userRole == "Administrador")
            {
                return Ok(post);
            }

            // Any authenticated user can see Public (2) + Private (4)
            if (userIdClaim != null)
            {
                if (post.EstadoId == 2 || post.EstadoId == 4)
                {
                    return Ok(post);
                }
                return Forbid();
            }

            // Anonymous users can only see Public (2)
            if (post.EstadoId == 2)
            {
                return Ok(post);
            }

            return Forbid();
        }

        [HttpPost]
        [Authorize(Roles = "Autor")]
        public async Task<ActionResult<PostDto>> CreatePost(CreatePostDto createPostDto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null) return Unauthorized();

                int userId = int.Parse(userIdClaim.Value);
                var command = new CreatePostCommand(createPostDto, userId);
                var result = await _mediator.Send(command);

                return CreatedAtAction(nameof(GetPost), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Autor")]
        public async Task<ActionResult<PostDto>> UpdatePost(int id, UpdatePostDto updatePostDto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null) return Unauthorized();

                int userId = int.Parse(userIdClaim.Value);
                // Only the Autor role can edit - IsAdmin check removed since only Autor should edit
                var command = new UpdatePostCommand(id, updatePostDto, userId, true);
                var result = await _mediator.Send(command);

                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Autor")]
        public async Task<ActionResult> DeletePost(int id)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null) return Unauthorized();

                int userId = int.Parse(userIdClaim.Value);
                // Only the Autor role can delete
                var command = new DeletePostCommand(id, userId, true);
                var result = await _mediator.Send(command);

                if (!result) return NotFound();

                return NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{id}/views")]
        public async Task<ActionResult<bool>> IncrementViews(int id)
        {
            try
            {
                var command = new IncrementPostViewsCommand(id);
                var result = await _mediator.Send(command);

                if (!result) return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
