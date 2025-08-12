using Assignment__Management_System.Models.Data;
using Assignment__Management_System.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Assignment__Management_System.Services
{
    public class CourseService
    {
        private readonly AppDbContext _context;

        public CourseService(AppDbContext context)
        {
            _context = context;
        }
        private IQueryable<Course> GetCourses(string instid)
        {
            var course = _context.Courses
                            .Where(c => c.InstId == instid);

            return course;
        }
    }
}
