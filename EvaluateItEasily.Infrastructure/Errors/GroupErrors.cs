namespace EvaluateItEasily.Infrastructure.Errors
{
    public static class GroupErrors
    {
        public static readonly Error NotFound = new(
            "Group.NotFound",
            "Group was not found",
            StatusCodes.Status404NotFound);

        public static readonly Error AlreadyHasGroup = new(
            "Group.AlreadyHasGroup",
            "You already lead a group",
            StatusCodes.Status409Conflict);

        public static readonly Error StudentAlreadyInGroup = new(
            "Group.StudentAlreadyInGroup",
            "This student is already in a group",
            StatusCodes.Status409Conflict);

        public static readonly Error MemberNotFound = new(
            "Group.MemberNotFound",
            "This student is not a member of the group",
            StatusCodes.Status404NotFound);

        public static readonly Error CannotRemoveLeader = new(
            "Group.CannotRemoveLeader",
            "Cannot remove the leader from the group",
            StatusCodes.Status400BadRequest);

        public static readonly Error NotLeader = new(
            "Group.NotLeader",
            "Only the group leader can perform this action",
            StatusCodes.Status401Unauthorized);

        public static readonly Error StudentNotFound = new(
            "Group.StudentNotFound",
            "Student with this email was not found",
            StatusCodes.Status404NotFound);

        public static readonly Error NoGroupFound = new(
            "Group.NoGroupFound",
            "You are not part of any group",
            StatusCodes.Status404NotFound);

        public static readonly Error CannotAddNonStudent = new(
            "Group.CannotAddNonStudent",
            "Only students can be added as group members",
                        StatusCodes.Status400BadRequest);

        public static readonly Error InvitationNotFound = new("Group.InvitationNotFound",
            "Invitation was not found",
                            StatusCodes.Status404NotFound);

        public static readonly Error InvitationAlreadySent = new("Group.InvitationAlreadySent",
            "An invitation has already been sent to this student",
                            StatusCodes.Status409Conflict);

        public static readonly Error InvitationAlreadyHandled = new("Group.InvitationAlreadyHandled",
            "This invitation has already been accepted or rejected",
                            StatusCodes.Status400BadRequest);

        public static readonly Error NotInvitedStudent = new("Group.NotInvitedStudent",
                "You are not the invited student for this invitation",
                            StatusCodes.Status401Unauthorized);
    }
}
