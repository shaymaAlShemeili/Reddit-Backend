namespace Reddit.DTOs
{
    public class BlogPostDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public List<CommentDTO>? Comments { get; set; }
        public int? CommentCount {get; set;}
    }
}
