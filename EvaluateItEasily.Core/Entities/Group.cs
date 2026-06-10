namespace EvaluateItEasily.Core.Entities
{
    public class Group : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string LeaderId { get; set; } = string.Empty;      
        public ApplicationUser Leader { get; set; } = default!;

        
        public ICollection<GroupMember> Members { get; set; } = [];
        public ICollection<GroupInvitation> Invitations { get; set; } = [];
        public Proposal? Proposal { get; set; }

    }
}
