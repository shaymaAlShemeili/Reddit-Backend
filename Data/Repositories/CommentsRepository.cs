using Reddit.Models;
using Reddit.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Reddit.Data.Repositories
{
    public class CommentsRepository : ICommentsRepository
    {
        private readonly RedditDBContext _context;

        public CommentsRepository(RedditDBContext context)
        {
            _context = context;
        }

        public async Task<CommentDTO> FindAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if(comment != null)
            {
                // You will have to map the Comment entity to CommentDTO
                return new CommentDTO 
                {
                    Id = comment.Id,
                    Content = comment.Content,
                    BlogPostId = comment.BlogPostId ?? 0,
                    UserId = comment.UserId,
                };
            }
            return null;
        }

        public Comment Add(CommentDTO commentDto)
        {
            var comment = new Comment 
            {
                Content = commentDto.Content,
                UserId = commentDto.UserId,
                BlogPostId = commentDto.BlogPostId
            };

            _context.Comments.Add(comment);
            return comment; // Return the comment entity
        }


        public void Update(CommentDTO commentDto)
        {
            // Fetch the existing comment
            var comment = _context.Comments.Find(commentDto.Id);

            if(comment != null)
            {
                // Update the entity
                comment.Content = commentDto.Content;

                _context.Comments.Update(comment);
            }
        }

        public void Remove(CommentDTO commentDto)
        {
            var comment = _context.Comments.Find(commentDto.Id);

            if(comment != null)
            {
                _context.Comments.Remove(comment);
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<CommentDTO>> GetAllAsync()
        {
             var comments = await _context.Comments.ToListAsync();

                var dtos = new List<CommentDTO>();

                foreach (var comment in comments)
                {
                    var user = await _context.ForumUsers.FindAsync(comment.UserId);
                    
                    var dto = new CommentDTO
                    {
                        Id = comment.Id,
                        Content = comment.Content,
                        UserId = comment.UserId,
                        BlogPostId = comment.BlogPostId ?? 0,
                        UserName = user.UserName ?? "Unknown"
                    };

                    dtos.Add(dto);
                }

                return dtos;
        }

        public async Task<List<CommentDTO>> GetByPostIdAsync(int postId)
        {
            var comments = await _context.Comments.ToListAsync();

                var dtos = new List<CommentDTO>();

                foreach (var comment in comments)
                {
                    var user = await _context.ForumUsers.FindAsync(comment.UserId);
                    
                    var dto = new CommentDTO
                    {
                        Id = comment.Id,
                        Content = comment.Content,
                        UserId = comment.UserId,
                        BlogPostId = comment.BlogPostId ?? 0,
                        UserName = user.UserName ?? "Unknown"
                    };

                    dtos.Add(dto);
                }

                return dtos;
        }

        public async Task<List<Comment>> GetByUserIdAsync(string userId)
        {
            return await _context.Comments.Where(c => c.UserId == userId).ToListAsync();
        }

        public async Task<int> GetCommentCountByPostIdAsync(int postId)
        {
            return await _context.Comments.CountAsync(c => c.BlogPostId == postId);
        }

    }
}