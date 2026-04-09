namespace EvaluateItEasily.Core.Entities
{
    public class GroupMember : AuditableEntity
    {
        public int Id { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; } = default!;

        public string StudentId { get; set; } = string.Empty;    
        public ApplicationUser Student { get; set; } = default!;

        public bool IsLeader { get; set; }
        public DateTime JoinedAt { get; set; }
    }
}
