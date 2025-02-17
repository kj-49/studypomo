using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using StudyPomo.Library.Data.Database;
using StudyPomo.Library.Data.Interfaces;
using StudyPomo.Library.Models.Identity;
using StudyPomo.Library.Models.Tables.CourseEntities;
using StudyPomo.Library.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StudyPomo.Library.Services;

public class CourseService : ICourseService
{
    private readonly IUserService _userService;
    private readonly ApplicationDbContext _context;

    public CourseService(IUserService userService, ApplicationDbContext context)
    {
        _userService = userService;
        _context = context;
    }

    public async Task ArchiveAsync(int id)
    {
        Course course = await _context.Courses.SingleAsync(u => u.Id == id);

        course.Archived = true;
        course.DateUpdated = DateTime.Now;

        await _context.SaveChangesAsync();
    }

    public async Task UnArchiveAsync(int id)
    {
        Course course = await _context.Courses.SingleAsync(u => u.Id == id);

        course.Archived = false;
        course.DateUpdated = DateTime.Now;

        await _context.SaveChangesAsync();
    }

    public async Task CreateAsync(CourseCreate courseCreate)
    {
        ApplicationUser? user = await _userService.GetCurrentUserAsync();
        if (user == null) throw new Exception("User not found");

        Course course = courseCreate.ToEntity(user.Id);

        await _context.Courses.AddAsync(course);

        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<Course>> GetAllAsync(int userId, bool includeArchived = false)
    {
        Expression<Func<Course, bool>> filter = u => u.UserId == userId;

        if (!includeArchived)
        {
            filter = u => u.UserId == userId && u.Archived == false;
        }

        return await _context.Courses
            .Where(filter)
            .Include(u => u.StudyTasks)
            .ToListAsync();
    }

    public async Task<Course> GetAsync(int id)
    {
        return await _context.Courses
            .Include(u => u.StudyTasks)
            .SingleAsync(u => u.Id == id);
    }

    public async Task RemoveAsync(int id)
    {
        Course course = await _context.Courses.SingleAsync(u => u.Id == id);

        _context.Courses.Remove(course);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CourseUpdate courseUpdate)
    {
        Course course = await _context.Courses.SingleAsync(u => u.Id == courseUpdate.Id);

        Course updatedCourse = courseUpdate.ToEntity(course);

        _context.Update(updatedCourse);

        await _context.SaveChangesAsync();
    }
}
