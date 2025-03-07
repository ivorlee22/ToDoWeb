namespace ToDoWeb.Application.Dtos
{
    public class GradesDetailModel
    {
        public int Id { get; set; }

        public String Name { get; set; }

        public List<CourseViewModel> courses { get; set; }
    }
}
