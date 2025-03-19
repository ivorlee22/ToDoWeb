using System.ComponentModel.DataAnnotations;

namespace ToDoWeb.Application.Dtos
{
    public class CourseUpdateModel
    {

        [Required]
        public int CourseId { get; set; }
        public string? CourseName { get; set; }
        public DateTime StartDate { get; set; }
    }
}
