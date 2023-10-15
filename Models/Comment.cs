namespace Reddit.Models;

public class Comment
{
    public Comment()
    {
        Content = string.Empty;
        UserId = string.Empty;
    }
    public int Id { get; set; }
    public string?  Content { get; set; }
    public int? BlogPostId { get; set; }
    public BlogPost?  BlogPost { get; set; }
    public string  UserId { get; set; }
    public ForumUser?  User { get; set; }
}
