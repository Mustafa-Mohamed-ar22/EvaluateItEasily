using EvaluateItEasily.Core.DTO_s.Groups;
using EvaluateItEasily.Core.Entities;
using Mapster;
namespace EvaluateItEasily.Infrastructure.Mapping
{
    public class MappingConfiguration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<GroupMember, GroupMemberResponse>()
                .Map(dest => dest.FullName, src => src.Student.FullName)
                .Map(dest => dest.Email, src => src.Student.Email);

            config.NewConfig<Group, GroupResponse>()
                .Map(dest => dest.LeaderName, src => src.Leader.FullName);
        }
    }
}
