using Reddit.DTOs;
using Reddit.Models;

namespace Reddit.Data.Repositories
{
    public interface IBlogPostRepository
    {
        Task<IEnumerable<BlogPost>> GetAllAsync(); 
        Task<BlogPost> GetAsync(int id);          
        Task<int> AddAsync(BlogPostDTO postDto);        
        Task UpdateAsync(BlogPostDTO postDto);     
        Task DeleteAsync(BlogPostDTO postDto);     
    }
}
