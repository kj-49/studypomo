using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudyPomo.Library.Models.Identity;
using StudyPomo.Library.Models.Tables.CourseEntities;
using StudyPomo.Library.Models.Tables.LabelEntities;
using StudyPomo.Library.Models.Tables.StudySessionEntities;
using StudyPomo.Library.Models.Tables.StudyTaskEntities;
using StudyPomo.Library.Models.Tables.StudyTaskLabelEntities;
using StudyPomo.Library.Models.Tables.StudyTypeEntities;
using StudyPomo.Library.Models.Tables.TaskPriorityEntities;
using System.Reflection.Emit;
namespace StudyPomo.Library.Data.Database;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>, IDataProtectionKeyContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public ApplicationDbContext() { }

    public DbSet<StudyTask> StudyTasks { get; set; }
    public DbSet<TaskPriority> TaskPriorities { get; set; }
    public DbSet<StudySession> StudySessions { get; set; }
    public DbSet<StudyType> StudyTypes { get; set; }
    public DbSet<TaskLabel> TaskLabels { get; set; }
    public DbSet<StudyTaskLabel> StudyTaskLabels { get; set; }
    public DbSet<Course> Courses { get; set; }

    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Course>(course =>
        {
            course.ToTable(nameof(Course));
        });

        builder.Entity<StudyTask>(studyTask =>
        {
            studyTask
                .ToTable(nameof(StudyTask));

            studyTask
                  .HasMany(e => e.TaskLabels)
                  .WithMany(e => e.StudyTasks)
                  .UsingEntity<StudyTaskLabel>(
                    u => u
                        .HasOne(t => t.TaskLabel)
                        .WithMany()
                        .HasForeignKey(t => t.TaskLabelId),
                    u => u
                        .HasOne(t => t.StudyTask)
                        .WithMany()
                        .HasForeignKey(t => t.StudyTaskId),
                    u =>
                        u
                        .HasKey(t => new { t.StudyTaskId, t.TaskLabelId })
                );

            studyTask
                .HasOne(e => e.User)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<StudyTaskLabel>(studyTaskLabel =>
        {
            studyTaskLabel
                .ToTable(nameof(StudyTaskLabel));

            studyTaskLabel
                .HasOne(u => u.StudyTask)
                .WithMany(u => u.StudyTaskLabels);
        });

        builder.Entity<TaskLabel>(taskLabel =>
        {
            taskLabel
                .ToTable(nameof(TaskLabel));

            taskLabel
                .HasOne(e => e.User)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<TaskPriority>(taskPriority =>
        {
            taskPriority
                .ToTable(nameof(TaskPriority))
                .HasData(
                new TaskPriority { Id = 1, Level = "Low", DisplayHexColor = "#28b54d" },
                new TaskPriority { Id = 2, Level = "Medium", DisplayHexColor = "#b57828" },
                new TaskPriority { Id = 3, Level = "High", DisplayHexColor = "#b52d28" });
        });

        builder.Entity<StudySession>(studySession =>
        {
            studySession.ToTable(nameof(StudySession));
        });

        builder.Entity<StudyType>(studyType =>
        {
            studyType.ToTable(nameof(StudyType)).HasData(
                new StudyType { Id = 1, TypeName = "Pomodoro" },
                new StudyType { Id = 2, TypeName = "Short Break" },
                new StudyType { Id = 3, TypeName = "Long Break" }
            );
        });
    }
}