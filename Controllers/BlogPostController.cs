using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reddit.Services;
using Reddit.DTOs;

namespace Reddit.Controllers
{
    [ApiController]
    [Route("api/blog/posts")]
    [Authorize]
    public class BlogPostController : ControllerBase
    {
        private readonly IBlogPostService _postService;

        public BlogPostController(IBlogPostService postService)
        {
            _postService = postService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(BlogPostDTO blogPostDto)
        {
            var postId = await _postService.CreatePostAsync(blogPostDto, User.Identity.Name);
            blogPostDto.Id = postId;

            return CreatedAtAction(nameof(GetPostById), new { id = postId, username = User.Identity.Name  }, blogPostDto);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllPosts()
        {
            return Ok(await _postService.GetAllPostsAsync());
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPostById(int id)
        {
            var blogPostDto = await _postService.GetPostByIdAsync(id);
            if (blogPostDto == null) return NotFound("Post not found");
            return Ok(blogPostDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditPost(int id, BlogPostDTO blogPostDto)
        {
            bool isUpdated = await _postService.EditPostAsync(id, blogPostDto, User.Identity.Name);
            if (!isUpdated) return Forbid("You are not authorized to update this post");
            return Ok(new { message = "Post updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            bool isDeleted = await _postService.DeletePostAsync(id, User.Identity.Name);
            if (!isDeleted) return Forbid("You are not authorized to delete this post");
            return Ok(new { message = "Post deleted successfully" });
        }
    }
}
