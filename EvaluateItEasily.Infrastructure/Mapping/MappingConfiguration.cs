using EvaluateItEasily.Core.DTO_s.Evaluations;
using EvaluateItEasily.Core.DTO_s.Groups;
using EvaluateItEasily.Core.DTO_s.Notifications;
namespace EvaluateItEasily.Infrastructure.Mapping
{
    public class MappingConfiguration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Notification, NotificationResponse>();

            config.NewConfig<GroupMember, GroupMemberResponse>()
                .Map(dest => dest.FullName, src => src.Student.FullName)
                .Map(dest => dest.Email, src => src.Student.Email);

            config.NewConfig<Group, GroupResponse>()
                .Map(dest => dest.LeaderName, src => src.Leader.FullName);

            TypeAdapterConfig<HistoricalProject, HistoricalProjectResponse>
            .NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Abstract, src => src.Abstract)
            .Map(dest => dest.GroupName, src => src.GroupName)
            .Map(dest => dest.AcademicYear, src => src.AcademicYear)
            .Map(dest => dest.ArchivedAt, src => src.ArchivedAt)
            .Map(dest => dest.ProposalId, src => src.ProposalId);

            // Evaluation → EvaluationResponse
            TypeAdapterConfig<Evaluation, EvaluationResponse>
                .NewConfig()
                .Map(dest => dest.ProposalTitle, src => src.Proposal.Title)
                .Map(dest => dest.EvaluatedByName, src => src.EvaluatedByUser.FullName)
                .Map(dest => dest.AIStatus, src => src.AIStatus.ToString())
                .Map(dest => dest.SimilarityResults, src => src.SimilarityResults);

            // SimilarityResult → SimilarityResultResponse
            TypeAdapterConfig<SimilarityResult, SimilarityResultResponse>
                .NewConfig()
                .Map(dest => dest.MatchedProjectName, src => src.HistoricalProject.Name)
                .Map(dest => dest.MatchedProjectAbstract, src => src.HistoricalProject.Abstract)
                .Map(dest => dest.MatchedProjectYear, src => src.HistoricalProject.AcademicYear);

            TypeAdapterConfig<Decision, DecisionResponse>
            .NewConfig()
            .Map(dest => dest.ProposalTitle, src => src.Proposal.Title)
            .Map(dest => dest.DecidedByName, src => src.DecidedByUser.FullName)
            .Map(dest => dest.DecisionType, src => src.DecisionType.ToString());


            TypeAdapterConfig<SupervisorAssignment, SupervisorAssignmentResponse>
            .NewConfig()
            .Map(dest => dest.ProposalTitle, src => src.Proposal.Title)
            .Map(dest => dest.GroupName, src => src.Proposal.Group.Name)
            .Map(dest => dest.SupervisorName, src => src.Supervisor.FullName)
            .Map(dest => dest.SupervisorEmail, src => src.Supervisor.Email)
            .Map(dest => dest.AssignedByName, src => src.AssignedByUser.FullName);
        }
    }
}