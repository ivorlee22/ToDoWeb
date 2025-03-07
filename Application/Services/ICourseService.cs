using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ToDoWeb.Application.Dtos;
using ToDoWeb.Domains.Entities;
using ToDoWeb.Infrastructures;

namespace ToDoWeb.Application.Services
{
    public interface ICourseService
    {
        StudentCourseViewModel GetCourseDetail(int id);

        IEnumerable<CourseViewModel> GetCourse(int? studentId);

        int PostCourse(CourseCreateModel course);
        int PutCourse(CourseUpdateModel course);
        void DeleteCourse(int courseId);

        void AssignCourse(int StudentId, int CourseId);
        int UpgradeGrades(int StudentId, int CourseId, float Assignment, float Final, float Practical);
        GradesDetailModel GetGradesDetail(int id);
        GPAViewModel GetGPA(int id);
    }
    public class CourseService : ICourseService
    {
        private readonly IApplicationDbContext _context;

        public CourseService(IApplicationDbContext context)
        {
            _context = context;
        }



        public IEnumerable<CourseViewModel> GetCourse(int? studentId)
        {
            var query = _context.CourseStudent
            .Include(c => c.Course)
            .Where(c => c.StudentId == studentId)
            .Select(c => new CourseViewModel
            {
                CourseId = c.Course.Id,
                CourseName = c.Course.Name,
                StartDate = c.Course.StartDate,
            })
            .ToList();

            return query;
        }

        public StudentCourseViewModel GetCourseDetail(int id)
        {
            var course = _context.Course.Find(id);
            if (course == null) return null;


            var candidate = _context.Student.FirstOrDefault(x => x.Id == id);
            var query = _context.CourseStudent
                .Where(x => x.StudentId == id)
                .Include(c => c.Course)
                .ToList();

            return new StudentCourseViewModel
            {
                StudentId = candidate.Id,
                StudentName = candidate.FirstName + " " + candidate.LastName,
                Course = query.Select(x => new CourseViewModel
                {
                    CourseId = x.Course.Id,
                    CourseName = x.Course.Name,
                    StartDate = x.Course.StartDate,

                }).ToList(),
            };
        }

        public int PostCourse(CourseCreateModel course)
        {
            if (_context.Course.Any(x => x.Id == course.Id))
            {
                Console.WriteLine($"Id {course.Id} already exists.");
                return course.Id;
            }
            var data = new Course
            {
                Name = course.Name,
                StartDate = course.StartDate,
            };
            _context.Course.Add(data);
            _context.SaveChanges();
            return data.Id;
        }

        public int PutCourse(CourseUpdateModel course)
        {
            var data = _context.Course.Find(course.Id);
            if (data == null) return -1;

            if (!string.IsNullOrEmpty(course.Name)) data.Name = course.Name;
            if (course.StartDate != null) data.StartDate = course.StartDate.Value;

            _context.SaveChanges();
            return data.Id;

        }
        public void DeleteCourse(int courseId)
        {
            var data = _context.Course.Find(courseId);
            if (data == null) return;

            _context.Course.Remove(data);
            _context.SaveChanges();
        }

        public void AssignCourse(int StudentId, int CourseId)
        {
            var student = _context.Student.Find(StudentId);
            var course = _context.Course.Find(CourseId);
            if (student == null || course == null) return;

            var isAssigned = _context.CourseStudent.Any(cs => cs.StudentId == student.Id && cs.CourseId == course.Id);
            if (isAssigned) return;

            var data = new CourseStudent
            {
                CourseId = CourseId,
                StudentId = StudentId
            };

            _context.CourseStudent.Add(data);
            _context.SaveChanges();
        }

        public int UpgradeGrades(int StudentId, int CourseId, float Assignment, float Final, float Practical)
        {
            var student = _context.Student.Find(StudentId);
            var course = _context.Course.Find(CourseId);
            var data = _context.CourseStudent.Find(CourseId, StudentId);
            if (student == null || course == null) return 0;
            if (data != null)
            {
                data.Assignment = Assignment;
                data.Final = Final;
                data.Practical = Practical;
                _context.SaveChanges();
                return data.StudentId;
            }
            return 0;
        }

        public GradesDetailModel GetGradesDetail(int id)
        {
            var student = _context.Student.Find(id); //DbSet<Courses>
            if (student == null) return null;

            var course = _context.CourseStudent
                .Include(x => x.Course)
                .Where(x => x.StudentId == id)
                .Select(x => new CourseViewModel
                {
                    CourseId = x.CourseId,
                    CourseName = x.Course.Name,
                    StartDate = x.Course.StartDate,
                    Assignment = x.Assignment,
                    Final = x.Final,
                    Practical = x.Practical
                }).ToList();


            return new GradesDetailModel
            {
                Id = student.Id,
                Name = student.FirstName + ' ' + student.LastName,
                courses = course
            };
        }

        public GPAViewModel GetGPA(int id)
        {
            var student = _context.Student.Find(id);
            if (student == null) return null;

            var gpa = _context.CourseStudent
                .Where(x => x.StudentId == id)
                .Average(x => (x.Practical + x.Final + x.Assignment) / 3);

            return new GPAViewModel
            {
                Id = student.Id,
                Name = student.FirstName + ' ' + student.LastName,
                Gpa = gpa
            };
        }
    }
}
