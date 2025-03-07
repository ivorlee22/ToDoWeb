using System.ComponentModel.DataAnnotations;

namespace ToDoWeb.Application.Dtos
{
    public class CourseUpdateModel
    {

        [Required]
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime? StartDate { get; set; }
    }
}
