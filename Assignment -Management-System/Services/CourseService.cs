using Assignment__Management_System.DataLayer.DTOs;
using Assignment__Management_System.Models.Data;
using Assignment__Management_System.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace Assignment__Management_System.Services
{
    public class CourseService : ICourseService
    {
        private readonly AppDbContext _context;

        public CourseService(AppDbContext context)
        {
            _context = context;
        }
        public IQueryable<Course> GetCourses(string instid)
        {
            var course = _context.Courses
                            .Where(c => c.InstId == instid);

            return course;
        }

        public string AddCourses(CourseDto model)
        {
            string Message = "";
            var result = _context.Courses.Any(c => c.CrsName == model.CrsName);

            if (!result)
            {
                Course course = new Course()
                {
                    CrsName = model.CrsName,
                    InstId = model.InstId,
                };

                try
                {
                    _context.Courses.Add(course);

                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Message = ex.Message;
                }

                return Message;
            }
            Message = "This Course already exist!";
            return Message;
        }

        public string EnrollCourse(CourseEnrollDTO model)
        {
            string Message = "";
            var result = _context.Courses.Any(c => c.CrsId == model.CrsId);
            if (result)
            {
                var course = new CourseEnrollments() { CrsId = model.CrsId, StuId = model.StuId};
                try
                {
                    _context.CourseEnrollments.Add(course);

                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Message = ex.Message;
                }
            }
            else
                Message = "Course is not found!";

            return Message;
        }
    }
}
