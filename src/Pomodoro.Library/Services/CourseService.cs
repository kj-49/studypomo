using Pomodoro.Library.Data.Interfaces;
using Pomodoro.Library.Models.Identity;
using Pomodoro.Library.Models.Tables.CourseEntities;
using Pomodoro.Library.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro.Library.Services;

public class CourseService : ICourseService
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;

    public CourseService(IUnitOfWork unitOfWork, IUserService userService)
    {
        _unitOfWork = unitOfWork;
        _userService = userService;
    }

    public async Task ArchiveAsync(int id)
    {
        Course? course = await _unitOfWork.Course.GetAsync(u => u.Id == id);
        if (course == null) throw new Exception("Course not found");
        
        course.Archived = true;
        course.DateUpdated = DateTime.Now;

        _unitOfWork.Course.Update(course);
        _unitOfWork.Complete();
    }

    public async Task CreateAsync(CourseCreate courseCreate)
    {
        ApplicationUser? user = await _userService.GetCurrentUserAsync();
        if (user == null) throw new Exception("User not found");

        Course course = courseCreate.ToEntity(user.Id);

        await _unitOfWork.Course.AddAsync(course);
        _unitOfWork.Complete();
    }

    public async Task<ICollection<Course>> GetAllAsync(int userId)
    {
        IEnumerable<Course> courses = await _unitOfWork.Course.GetAllAsync(
            filter: u => u.UserId == userId,
            includeProperties: t => t.StudyTasks);
        return courses.ToList();
    }

    public async Task<Course> GetAsync(int id)
    {
        Course? course = await _unitOfWork.Course.GetAsync(
            filter: u => u.Id == id,
            includeProperties: t => t.StudyTasks);
        if (course == null) throw new Exception("Course not found");
        return course;
    }

    public async Task RemoveAsync(int id)
    {
        Course? course = await _unitOfWork.Course.GetAsync(u => u.Id == id);
        if (course == null) throw new Exception("Course not found");

        _unitOfWork.Course.Remove(course);
        _unitOfWork.Complete();
    }

    public async Task UpdateAsync(CourseUpdate courseUpdate)
    {
        Course? course = await _unitOfWork.Course.GetAsync(u => u.Id == courseUpdate.Id);
        if (course == null) throw new Exception("Course not found");

        Course updatedCourse = courseUpdate.ToEntity(course);
        _unitOfWork.Course.Update(updatedCourse);
        _unitOfWork.Complete();
    }
}
