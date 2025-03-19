using System.Collections.Immutable;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public CourseService(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }



        public IEnumerable<CourseViewModel> GetCourse(int? id)
        {

            //var courses = _context.Course.AsNoTracking().ToList();
            //courses[0].Name = "fafasd"; //Detached 
            //_context.SaveChanges();
            ////nếu code như trên thì EF sẽ k cần theo dõi nữa => tăng performance của chương trình

            //var query = _context.CourseStudent
            //.Include(c => c.Course)
            //.Where(c => c.StudentId == studentId)
            //.Select(c => new CourseViewModel
            //{
            //    Id = c.Course.Id,
            //    Name = c.Course.Name,
            //    StartDate = c.Course.StartDate,
            //})
            //.ToList();


            var query = _context.Course.AsQueryable();
            if (id.HasValue)
            {
                query = query.Where(course => course.Id == id);
                if (query.Count() == 0) return null;
            }
            //List<Course> courses = query.ToList();

            //var result = courses
            //.Select(x => _mapper.Map<CourseViewModel>(x))
            //.ToList();
            //var result = _mapper.Map<List<CourseViewModel>>(courses);

            var result = _mapper.ProjectTo<CourseViewModel>(query).ToList();

            return result;
        }

        public StudentCourseViewModel GetCourseDetail(int id)
        {
            var student = _context.Student.Include(s => s.CourseStudents)
                                   .ThenInclude(cs => cs.Course)
                                   .FirstOrDefault(s => s.Id == id);

            if (student == null) return null;

            return _mapper.Map<StudentCourseViewModel>(student);
        }

        public int PostCourse(CourseCreateModel course)
        {
            var data = _mapper.Map<Course>(course);
            _context.Course.Add(data);
            _context.SaveChanges();
            return data.Id;
        }

        public int PutCourse(CourseUpdateModel course)
        {
            var oldCourse = _context.Course.Find(course.CourseId);
            if (oldCourse == null) return -1;

            //if (!string.IsNullOrEmpty(course.Name)) oldCourse.Name = course.Name;
            //if (course.StartDate != null) oldCourse.StartDate = course.StartDate.Value;

            _mapper.Map(course, oldCourse);
            //_mapper.Map<Course>(course);
            _context.SaveChanges();
            return oldCourse.Id;

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
