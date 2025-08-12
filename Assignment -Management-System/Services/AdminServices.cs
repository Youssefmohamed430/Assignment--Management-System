using Assignment__Management_System.DataLayer.DTOs;
using Assignment__Management_System.Models.Data;
using Assignment__Management_System.Models.Entities;

namespace Assignment__Management_System.Services
{
    public class AdminServices : IAdminService
    {
        private readonly AppDbContext _context;

        public AdminServices(AppDbContext context)
        {
            _context = context;
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

                    Message = "Course is adding Succefully";
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
    }
}
