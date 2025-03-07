using System.ComponentModel.DataAnnotations;

namespace ToDoWeb.Application.Dtos
{
    public class AssignCourseModel
    {
        [Required]
        public int StudentId { get; set; }
        public int CourseId { get; set; }
    }
}
