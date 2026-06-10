
namespace EvaluateItEasily.Core.Entities
{
    public class SubmissionPeriod : AuditableEntity
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;   
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; } = true;


        public bool IsOpen => IsActive
            && DateTime.UtcNow >= StartDate
            && DateTime.UtcNow <= EndDate;
    }
}
