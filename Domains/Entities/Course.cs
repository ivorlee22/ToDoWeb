using ToDoWeb.Domains.Interfaces;

namespace ToDoWeb.Domains.Entities
{
    public class Course : ICreatedAt, ICreatedBy, IUpdatedBy, IUpdatedAt
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public DateTime StartDate { get; set; }
        public virtual ICollection<CourseStudent> CourseStudents { get; set; }

        public int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
