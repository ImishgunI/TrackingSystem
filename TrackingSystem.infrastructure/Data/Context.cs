using Microsoft.EntityFrameworkCore;
using TrackingSystem.Domain.Entities;

namespace TrackingSystem.Infrastructure.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Attendance> Attendances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("Students");
                entity.Property(s => s.Id);

                entity.HasKey(s => s.Id);
                entity.HasIndex(s => s.StudentIdNumber).IsUnique();
                entity.Property(s => s.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(s => s.LastName).IsRequired().HasMaxLength(100);
                entity.Property(s => s.MiddleName).HasMaxLength(100);
                entity.Property(s => s.StudentIdNumber).IsRequired().HasMaxLength(20);
                entity.Property(s => s.Email).HasMaxLength(255);
                entity.Property(s => s.PhoneNumber).HasMaxLength(20);
                entity.Property(s => s.GroupId);

                entity.HasOne(s => s.Group)
                      .WithMany(g => g.Students)
                      .HasForeignKey(s => s.GroupId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.ToTable("Groups");
                entity.Property(g => g.Id);
                entity.HasKey(g => g.Id);
                entity.HasIndex(g => g.Name).IsUnique();
                entity.Property(g => g.Name).IsRequired().HasMaxLength(50);
                entity.Property(g => g.Specialization).HasMaxLength(100);
            });

            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.ToTable("Teachers");
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Id);
                entity.Property(t => t.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(t => t.LastName).IsRequired().HasMaxLength(100);
                entity.Property(t => t.MiddleName).HasMaxLength(100);
                entity.Property(t => t.Position).IsRequired().HasMaxLength(50);
                entity.Property(t => t.Email).HasMaxLength(255);
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.ToTable("Subjects");
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Id);
                entity.Property(s => s.Name).IsRequired().HasMaxLength(100);
                entity.Property(s => s.Code).HasMaxLength(10);
                entity.Property(s => s.Description).HasMaxLength(500);

                entity.HasOne(s => s.Teacher)
                      .WithMany(t => t.Subjects)
                      .HasForeignKey(s => s.TeacherId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Lesson>(entity =>
            {
                entity.ToTable("Lessons");
                entity.Property(l => l.Id);
                entity.HasKey(l => l.Id);
                entity.Property(l => l.Topic).IsRequired().HasMaxLength(200);

                entity.HasOne(l => l.Subject)
                      .WithMany(s => s.Lessons)
                      .HasForeignKey(l => l.SubjectId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(l => l.Group)
                      .WithMany(g => g.Lessons)
                      .HasForeignKey(l => l.GroupId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Attendance>(entity =>
            {
                entity.ToTable("Attendance");
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Id);
                entity.HasIndex(a => new { a.StudentId, a.LessonId }).IsUnique();

                entity.Property(a => a.Notes).HasMaxLength(500);
                entity.Property(a => a.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

                entity.HasOne(a => a.Student)
                      .WithMany(s => s.Attendances)
                      .HasForeignKey(a => a.StudentId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(a => a.Lesson)
                      .WithMany(l => l.Attendances)
                      .HasForeignKey(a => a.LessonId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}