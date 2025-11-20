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
            var posts = await _mediator.Send(new GetPostsQuery(search));
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
            return Ok(post);
        }

        [HttpGet("slug/{slug}")]
        public async Task<ActionResult<PostDto>> GetPostBySlug(string slug)
        {
            var post = await _mediator.Send(new GetPostBySlugQuery(slug));
            if (post == null) return NotFound();

            // Visibility Logic
            // 1: Borrador (Saved) - Only Creator
            // 2: Publicado (Public) - Everyone
            // 3: Archivado - Only Creator (or Admin)
            // 4: Privado - Only Creator (or Admin)

            if (post.EstadoId == 2) return Ok(post);

            // Check authentication for non-public posts
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Forbid(); // Or NotFound to hide existence

            int userId = int.Parse(userIdClaim.Value);
            var isAdmin = User.IsInRole("Administrador");

            // Allow if Admin or Creator
            if (isAdmin || post.AutorId == userId)
            {
                return Ok(post);
            }

            return Forbid();
        }

        [HttpPost]
        [Authorize(Roles = "Autor,Administrador")]
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
        [Authorize(Roles = "Autor,Administrador")]
        public async Task<ActionResult<PostDto>> UpdatePost(int id, UpdatePostDto updatePostDto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null) return Unauthorized();

                int userId = int.Parse(userIdClaim.Value);
                var isAdmin = User.IsInRole("Administrador");

                var command = new UpdatePostCommand(id, updatePostDto, userId, isAdmin);
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
        [Authorize(Roles = "Autor,Administrador")]
        public async Task<ActionResult> DeletePost(int id)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null) return Unauthorized();

                int userId = int.Parse(userIdClaim.Value);
                var isAdmin = User.IsInRole("Administrador");

                var command = new DeletePostCommand(id, userId, isAdmin);
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
    }
}
