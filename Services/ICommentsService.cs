using Reddit.Models;
using Reddit.DTOs;

namespace Reddit.Services
{
    public interface ICommentsService
    {
        Task<CommentDTO> GetCommentAsync(int commentId);
        Task<int> AddCommentAsync(CommentDTO commentDto);
        Task UpdateCommentAsync(CommentDTO commentDto);
        Task DeleteCommentAsync(CommentDTO commentDto);
        Task<ForumUser> GetUserByUsernameAsync(string username);
        Task<List<CommentDTO>> GetAllCommentsAsync();
        Task<List<CommentDTO>> GetCommentsByPostIdAsync(int postId);
        Task<List<CommentDTO>> GetCurrentUsersCommentsAsync(string userId);
    }
}
