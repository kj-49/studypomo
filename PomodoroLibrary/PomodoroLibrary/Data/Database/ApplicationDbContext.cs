using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PomodoroLibrary.Models.Identity;
using PomodoroLibrary.Models.Tables.StudySessionEntities;
using PomodoroLibrary.Models.Tables.StudyTaskEntities;
using PomodoroLibrary.Models.Tables.StudyTypeEntities;
using PomodoroLibrary.Models.Tables.TaskPriorityEntities;
namespace PomodoroLibrary.Data.Database;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<StudyTask> Tasks { get; set; }
    public DbSet<TaskPriority> TaskPriorities { get; set; }
    public DbSet<StudySession> StudySessions { get; set; }
    public DbSet<StudyType> StudyTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);


        builder.Entity<StudyTask>().ToTable(nameof(StudyTask))
            .HasOne(u => u.TaskPriority);

        builder.Entity<TaskPriority>().ToTable(nameof(TaskPriority))
            .HasData(
            new TaskPriority { Id = 1, Level = "Low" , DisplayHexColor = "#28b54d" },
            new TaskPriority { Id = 2, Level = "Medium", DisplayHexColor = "#b57828" },
            new TaskPriority { Id = 3, Level = "High", DisplayHexColor = "#b52d28" });

        builder.Entity<StudySession>().ToTable(nameof(StudySession));

        builder.Entity<StudyType>().ToTable(nameof(StudyType)).HasData(
            new StudyType { Id = 1, TypeName = "Pomodoro" },
            new StudyType { Id = 2, TypeName = "Short Break"},
            new StudyType { Id = 3, TypeName = "Long Break" }
        );

    }


}