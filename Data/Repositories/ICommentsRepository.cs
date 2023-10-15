using Reddit.DTOs;
using Reddit.Models;

namespace Reddit.Data.Repositories
{
    public interface ICommentsRepository
    {
        Task<CommentDTO> FindAsync(int id);
        Comment Add(CommentDTO commentDto);
        void Update(CommentDTO commentDto);
        void Remove(CommentDTO commentDto);
        Task SaveChangesAsync();
        Task<List<CommentDTO>> GetAllAsync();
        Task<List<CommentDTO>> GetByPostIdAsync(int postId);
        Task<List<Comment>> GetByUserIdAsync(string userId);
        Task<int> GetCommentCountByPostIdAsync(int postId);
    }
}
