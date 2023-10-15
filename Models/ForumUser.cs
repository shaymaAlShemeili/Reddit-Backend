namespace Reddit.Models
{
    using Microsoft.AspNetCore.Identity;

    public class ForumUser : IdentityUser
    {
        public ICollection<Comment> Comments { get; set; }
        // Add any additional properties here that are not included in IdentityUser
        // No need to redefine Id, UserName, Email, and PasswordHash
    }
}
