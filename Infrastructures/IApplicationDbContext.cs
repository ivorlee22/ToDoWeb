using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ToDoWeb.Domains.Entities;

namespace ToDoWeb.Infrastructures
{
    public interface IApplicationDbContext
    {
        public DbSet<ToDo> ToDos { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<CourseStudent> CourseStudent { get; set; }
        public DbSet<School> School { get; set; }
        public EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        public int SaveChanges();
    }
}
