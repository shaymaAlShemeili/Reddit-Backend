using Reddit.DTOs;
using Reddit.Models;

namespace Reddit.Services
{
    public interface IBlogPostService
    {
        Task<int> CreatePostAsync(BlogPostDTO blogPostDto, string username);
        Task<bool> EditPostAsync(int id, BlogPostDTO blogPostDto, string username);
        Task<List<BlogPostDTO>> GetAllPostsAsync();
        Task<bool> DeletePostAsync(int id, string username);
        Task<BlogPostDTO> GetPostByIdAsync(int id);
    }
}
