using System.ComponentModel.DataAnnotations;

namespace ToDoWeb.Application.Dtos
{
    public class CourseCreateModel
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }

    }
}
