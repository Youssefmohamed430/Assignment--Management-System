using Assignment__Management_System.DataLayer.DTOs;
using Assignment__Management_System.Models.Data;
using Assignment__Management_System.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Assignment__Management_System.Services
{
    public class StudentService : IStudentService
    {
        private AppDbContext context;

        public StudentService(AppDbContext context)
        {
            this.context = context;
        }

        public IQueryable<CourseEnrollDTO> GetCourseEnrollments(string studentid)
        {
            var CourseEnrolls = context.CourseEnrollments.AsNoTracking()
                .Include(e => e.course)
                .Where(e => e.StuId == studentid)
                .Select(x => new CourseEnrollDTO {
                    CrsId = x.CrsId,
                    StuId = x.StuId,
                });

            return CourseEnrolls;
        }

        public IQueryable<AssignmentSubsDetails> GetAssignmentDetails(int assignid, string studid)
        {
            var Assignsub = context.Submissions.AsNoTracking()
                .Include(e => e.assignment)
                .Where(s => s.AssignmentId == assignid && s.StuId == studid)
                .Select(x => new AssignmentSubsDetails()
                {
                    Title = x.assignment.Title,
                    DeadLine = x.assignment.DeadLine,
                    FileName = Path.GetFileName(x.FilePath),
                    grade = x.grade
                });

            return Assignsub;
        }
    }
}
