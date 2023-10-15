using System.Collections.Generic; // Required for List<T>
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reddit.Models
{
    public class BlogPost
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;
        
        public string Content { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        // Navigation property for the user
        public ForumUser? User { get; set; }

        // Navigation property for comments. Initialized here.
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
