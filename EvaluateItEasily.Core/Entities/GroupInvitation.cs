using EvaluateItEasily.Core.Enums;

namespace EvaluateItEasily.Core.Entities
{
    public class GroupInvitation : AuditableEntity
    {
        public int Id { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; } = default!;

        public string InvitedStudentId { get; set; } = string.Empty;
        public ApplicationUser InvitedStudent { get; set; } = default!;

        public InvitationStatus Status { get; set; } = InvitationStatus.Pending;
        public DateTime? RespondedAt { get; set; }
    }
}
