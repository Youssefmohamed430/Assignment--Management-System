using Assignment__Management_System.DataLayer.DTOs;
using Assignment__Management_System.Models.Entities;

namespace Assignment__Management_System.Services
{
    public interface ICourseService
    {
        string AddCourses(CourseDto model);
        string EnrollCourse(CourseEnrollDTO model);
        //IQueryable<Course> GetCourses(string instid);
    }
}
