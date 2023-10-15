using Microsoft.EntityFrameworkCore;
using Reddit.Models;
using Reddit.DTOs;

namespace Reddit.Data.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly RedditDBContext _context;
        private readonly ILogger<BlogPostRepository> _logger;

        public BlogPostRepository(RedditDBContext context, ILogger<BlogPostRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<BlogPost> GetAsync(int id)
        {
            return await _context.BlogPosts.FindAsync(id);
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await _context.BlogPosts.ToListAsync();
        }

        public async Task<int> AddAsync(BlogPostDTO postDto)
        {
            var blogPost = new BlogPost 
            {
                Title = postDto.Title,
                Content = postDto.Content,
                UserId = postDto.UserId
            };
            
            _context.BlogPosts.Add(blogPost);
            await _context.SaveChangesAsync();

            return blogPost.Id;
        }

        public async Task UpdateAsync(BlogPostDTO postDto)
        {
            var blogPost = await _context.BlogPosts.FindAsync(postDto.Id);

            if (blogPost != null)
            {
                blogPost.Title = postDto.Title;
                blogPost.Content = postDto.Content;
                _context.Entry(blogPost).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(BlogPostDTO postDto)
        {
            var blogPost = await _context.BlogPosts.FindAsync(postDto.Id);
            if (blogPost != null)
            {
                _context.BlogPosts.Remove(blogPost);
                await _context.SaveChangesAsync();
            }
        }
    }
}
