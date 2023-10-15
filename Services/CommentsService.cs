using Reddit.DTOs;
using Reddit.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Reddit.Models;

namespace Reddit.Services
{
    public class CommentsService : ICommentsService
    {
        private readonly ICommentsRepository _commentRepo;
        private readonly UserManager<ForumUser> _userManager;

        public CommentsService(ICommentsRepository commentRepo, UserManager<ForumUser> userManager)
        {
            _commentRepo = commentRepo;
            _userManager = userManager;
        }

        public async Task<CommentDTO> GetCommentAsync(int commentId)
        {
            return await _commentRepo.FindAsync(commentId);
        }

        public async Task<int> AddCommentAsync(CommentDTO commentDto)
        {
            var addedComment = _commentRepo.Add(commentDto);
            await _commentRepo.SaveChangesAsync();
            return addedComment.Id;
        }

        public async Task UpdateCommentAsync(CommentDTO commentDto)
        {
            _commentRepo.Update(commentDto);
            await _commentRepo.SaveChangesAsync();
        }

        public async Task DeleteCommentAsync(CommentDTO commentDto)
        {
            _commentRepo.Remove(commentDto);
            await _commentRepo.SaveChangesAsync();
        }

        public async Task<ForumUser> GetUserByUsernameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public async Task<List<CommentDTO>> GetAllCommentsAsync()
        {
            var comments = await _commentRepo.GetAllAsync();
            return comments.Select(c => new CommentDTO
            {
                Id = c.Id,
                Content = c.Content,
                UserId = c.UserId,
                BlogPostId = c.BlogPostId
            }).ToList();
        }

        public async Task<List<CommentDTO>> GetCommentsByPostIdAsync(int postId)
        {
            var comments = await _commentRepo.GetByPostIdAsync(postId);
            return comments.Select(c => new CommentDTO
            {
                Id = c.Id,
                Content = c.Content,
                UserId = c.UserId,
                BlogPostId = c.BlogPostId,
                UserName = c.UserName
            }).ToList();
        }

        public async Task<List<CommentDTO>> GetCurrentUsersCommentsAsync(string userId)
        {
            var comments = await _commentRepo.GetByUserIdAsync(userId);
            return comments.Select(c => new CommentDTO
            {
                Id = c.Id,
                Content = c.Content,
                UserId = c.UserId,
                BlogPostId = c.BlogPostId ?? 0
            }).ToList();
        }

    }
}
