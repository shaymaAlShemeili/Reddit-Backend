using Reddit.Models;
using Reddit.Data.Repositories;
using Reddit.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Reddit.Services
{
    public class BlogPostService : IBlogPostService
    {
        private readonly IBlogPostRepository _postRepo;
        private readonly ICommentsRepository _commentsRepo;

        private readonly UserManager<ForumUser> _userManager;

        public BlogPostService(IBlogPostRepository postRepo, ICommentsRepository commentRepo, UserManager<ForumUser> userManager)
        {
            _postRepo = postRepo;
            _userManager = userManager;
            _commentsRepo = commentRepo;
        }

        public async Task<int> CreatePostAsync(BlogPostDTO blogpPostDto, string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if(user == null)
            {
                throw new Exception($"User {username} not found.");
            }
            blogpPostDto.UserId = user.Id.ToString();
            blogpPostDto.UserName = username;
            int newPostId = await _postRepo.AddAsync(blogpPostDto); 
            return newPostId;
        }


        public async Task<bool> EditPostAsync(int id, BlogPostDTO blogpPostDto, string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            var postFromDb = await _postRepo.GetAsync(id);
            if (postFromDb.UserId != user.Id.ToString())
            {
                return false;
            }
            blogpPostDto.Id = id;
            await _postRepo.UpdateAsync( blogpPostDto);

            return true;
        }

        public async Task<List<BlogPostDTO>> GetAllPostsAsync()
        {
            var posts = await _postRepo.GetAllAsync();
            var postDTOs = new List<BlogPostDTO>();

            foreach (var post in posts)
            {
                var user = await _userManager.FindByIdAsync(post.UserId);
                var commentsCount = await _commentsRepo.GetCommentCountByPostIdAsync(post.Id); // use your comments repository or service here
                
                var postDto = new BlogPostDTO
                {
                    Id = post.Id,
                    Title = post.Title,
                    Content = post.Content,
                    UserId = post.UserId,
                    UserName = user?.UserName,
                    CommentCount = commentsCount
                };

                postDTOs.Add(postDto);
            }

            return postDTOs;
        }

        public async Task<bool> DeletePostAsync(int id, string username)
        {
            var postDto = new BlogPostDTO { Id = id };
            var user = await _userManager.FindByNameAsync(username);
            
            await _postRepo.DeleteAsync(postDto);

            return true;
        }

        public async Task<BlogPostDTO> GetPostByIdAsync(int id)
        {
            var post = await _postRepo.GetAsync(id);
            if (post == null) return null;

            var user = await _userManager.FindByIdAsync(post.UserId);
            
            return new BlogPostDTO
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                UserId = post.UserId,
                UserName = user?.UserName

            };
        }

    }
}
