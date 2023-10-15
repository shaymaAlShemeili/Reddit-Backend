using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Reddit.Services;
using Reddit.DTOs;
using Reddit.Models;

namespace Reddit.Controllers
{
    [ApiController]
    [Route("api/comments")]
    [Authorize]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentsService _CommentsService;
        private readonly UserManager<ForumUser> _userManager;

        public CommentsController(ICommentsService CommentsService, UserManager<ForumUser> userManager)
        {
            _CommentsService = CommentsService;
            _userManager = userManager;
        }

        [HttpPost("post/{postId}")]
        public async Task<IActionResult> AddComment(int postId, CommentDTO commentDto)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound("User not found");
            }

            commentDto.UserId = user.Id;
            commentDto.BlogPostId = postId;
            int newCommentId = await _CommentsService.AddCommentAsync(commentDto);
            commentDto.Id = newCommentId;
            return CreatedAtAction(nameof(GetComment), new { postId, commentId = newCommentId }, commentDto);
        }

        [HttpPut("{commentId}")]
        public async Task<IActionResult> UpdateComment(int postId, int commentId, CommentDTO updatedCommentDto)
        {
            var commentDto = await _CommentsService.GetCommentAsync(commentId);

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null || user.Id != commentDto.UserId)
            {
                return Forbid("You are not authorized to update this comment");
            }

            await _CommentsService.UpdateCommentAsync(updatedCommentDto);

            return NoContent();
        }

        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComment(int postId, int commentId)
        {
            var commentDto = await _CommentsService.GetCommentAsync(commentId);
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null || user.Id != commentDto.UserId)
            {
                return Forbid("You are not authorized to delete this comment");
            }

            await _CommentsService.DeleteCommentAsync(commentDto);

            return NoContent();
        }

        [HttpGet("{commentId}")]
        public async Task<ActionResult<CommentDTO>> GetComment(int postId, int commentId)
        {
            var commentDto = await _CommentsService.GetCommentAsync(commentId);
            if (commentDto == null || commentDto.BlogPostId != postId)
            {
                return NotFound("Comment not found or mismatched with post.");
            }

            return commentDto;
        }

        [HttpGet]
        public async Task<ActionResult<List<CommentDTO>>> GetAllComments()
        {
            var comments = await _CommentsService.GetAllCommentsAsync();
            return Ok(comments);
        }

        [HttpGet("post/{postId}")]
        public async Task<ActionResult<List<CommentDTO>>> GetCommentsByPostId(int postId)
        {
            var comments = await _CommentsService.GetCommentsByPostIdAsync(postId);
            return Ok(comments);
        }

        [HttpGet("user/current")]
        public async Task<ActionResult<List<CommentDTO>>> GetCurrentUsersComments()
        {
            var userId = _userManager.GetUserId(User);
            var comments = await _CommentsService.GetCurrentUsersCommentsAsync(userId);
            return Ok(comments);
        }

    }
}
