using Microsoft.AspNetCore.Mvc;
using ToDoApp.Application.Dtos;
using ToDoWeb.Application.Dtos;
using ToDoWeb.Application.Services;

namespace ToDoWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }
        [HttpGet("{id}")]
        public StudentCourseViewModel GetCourseDetail(int id)
        {
            return _courseService.GetCourseDetail(id);
        }
        [HttpGet]
        public IEnumerable<CourseViewModel> GetCourse(int? studentId)
        {
            return _courseService.GetCourse(studentId);
        }

        [HttpPost]
        public int PostCourse(CourseCreateModel course)
        {
            return _courseService.PostCourse(course);
        }

        [HttpPut]
        public int PutCourse(CourseUpdateModel course)
        {
            return _courseService.PutCourse(course);
        }

        [HttpDelete]
        public void DeleteCourse(int courseId)
        {
            _courseService.DeleteCourse(courseId);
        }

        [HttpPost("assign")]
        public void AssignCourse(int StudentId, int CourseId)
        {
            _courseService.AssignCourse(StudentId, CourseId);
        }

        [HttpPut("grades")]
        public int UpgradeGrades(int StudentId, int CourseId, float Assignment, float Final, float Practical)
        {
            return _courseService.UpgradeGrades(StudentId, CourseId, Assignment, Final, Practical);
        }

        [HttpGet("gradesDetail")]
        public GradesDetailModel GetGradesDetail(int id)
        {
            return _courseService.GetGradesDetail(id);
        }
        [HttpGet("Gpa")]
        public GPAViewModel GetGPA(int id)
        {
            return _courseService.GetGPA(id);
        }
    }
}
