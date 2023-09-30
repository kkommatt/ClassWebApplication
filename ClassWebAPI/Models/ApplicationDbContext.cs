using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
namespace ClassWebAPI.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Lector> Lectors { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<LectorCourse> LectorCourses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentCourse>()
                .HasKey(sc => new { sc.StudentId, sc.CourseId });

            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Student)
                .WithMany(s => s.StudentCourses)
                .HasForeignKey(sc => sc.StudentId);

            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Course)
                .WithMany(c => c.StudentCourses)
                .HasForeignKey(sc => sc.CourseId);

            modelBuilder.Entity<LectorCourse>()
                .HasKey(lc => new { lc.LectorId, lc.CourseId });

            modelBuilder.Entity<LectorCourse>()
                .HasOne(lc => lc.Lector)
                .WithMany(l => l.LectorCourses)
                .HasForeignKey(lc => lc.LectorId);

            modelBuilder.Entity<LectorCourse>()
                .HasOne(lc => lc.Course)
                .WithMany(c => c.LectorCourses)
                .HasForeignKey(lc => lc.CourseId);

            base.OnModelCreating(modelBuilder);
        }
        

    }

}
