namespace ToDoWeb.Domains.Entities
{
    public class CourseAuditLog
    {
        public int Id { get; set; }
        public string EntityName { get; set; }
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public string Action { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
