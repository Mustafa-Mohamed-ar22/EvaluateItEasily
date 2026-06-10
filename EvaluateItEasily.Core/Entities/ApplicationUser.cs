using Microsoft.AspNetCore.Identity;
namespace EvaluateItEasily.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedOn { get; set; }
        public ICollection<GroupMember> GroupMemberships { get; set; } = [];
        public Group? LeadingGroup { get; set; }
        public ICollection<Notification> Notifications { get; set; } = [];
        public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
        public ICollection<SupervisorAssignment> SupervisedProjects { get; set; } = [];
    }
}
