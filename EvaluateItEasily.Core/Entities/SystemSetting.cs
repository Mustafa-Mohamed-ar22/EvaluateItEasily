namespace EvaluateItEasily.Core.Entities
{
    public class SystemSetting : AuditableEntity
    {
        public int Id { get; set; }
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}
