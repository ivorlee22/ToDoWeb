using System.Net.WebSockets;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ToDoWeb.Domains.Entities;
using ToDoWeb.Infrastructures.DatabaseMapping;

namespace ToDoWeb.Infrastructures
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<ToDo> ToDos { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<CourseStudent> CourseStudent { get; set; }
        public DbSet<School> School { get; set; }
        public DbSet<AuditLog> AuditLog { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.UseSqlServer("Server=IVORLEE\\SQLEXPRESS ; Database=ToDoApp;Trusted_Connection=True;TrustServerCertificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .Property(x => x.Age)
                .HasComputedColumnSql("DATEDIFF(YEAR, DateOfBirth, GETDATE())");

            modelBuilder.Entity<Student>()
                .HasMany(student => student.CourseStudents)
                .WithOne(courseStudent => courseStudent.Student)
                .HasForeignKey(courseStudent => courseStudent.StudentId);

            modelBuilder.Entity<Course>()
                .HasMany(course => course.CourseStudents)
                .WithOne(courseStudent => courseStudent.Course)
                .HasForeignKey(courseStudent => courseStudent.CourseId);

            modelBuilder.Entity<CourseStudent>()
                .HasKey(courseStudent => new { courseStudent.CourseId, courseStudent.StudentId });

            modelBuilder.ApplyConfiguration(new CourseMapping());
            base.OnModelCreating(modelBuilder);
        }

        public int SaveChanges()
        {
            var auditLogs = new List<AuditLog>();
            foreach (var entity in ChangeTracker.Entries())
            {
                var log = new AuditLog
                {
                    EntityName = entity.Entity.GetType().Name,
                    CreatedAt = DateTime.Now,
                    Action = entity.State.ToString(),

                };
                if (entity.State == EntityState.Added)
                {
                    log.NewValue = JsonSerializer.Serialize(entity.CurrentValues.ToObject());
                }
                if (entity.State == EntityState.Modified)
                {
                    log.OldValue = JsonSerializer.Serialize(entity.OriginalValues.ToObject());
                    log.NewValue = JsonSerializer.Serialize(entity.CurrentValues.ToObject());
                }
                if (entity.State == EntityState.Deleted)
                {
                    log.OldValue = JsonSerializer.Serialize(entity.OriginalValues.ToObject());
                }
                auditLogs.Add(log);
            }
            AuditLog.AddRange(auditLogs);
            return base.SaveChanges();
        }

        public EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class
        {
            return base.Entry(entity);
        }

    }
}
