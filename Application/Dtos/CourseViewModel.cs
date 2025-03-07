namespace ToDoWeb.Application.Dtos
{
    public class CourseViewModel
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public DateTime StartDate { get; set; }

        public float Assignment { get; set; }
        public float Final { get; set; }
        public float Practical { get; set; }
    }
}
