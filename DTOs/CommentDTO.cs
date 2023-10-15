namespace Reddit.DTOs
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string? UserId { get; set; }
        public int BlogPostId { get; set; }
        public string? UserName { get; set; }
    }
}
