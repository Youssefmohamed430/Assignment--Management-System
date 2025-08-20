using Assignment__Management_System.DataLayer;
using Assignment__Management_System.DataLayer.DTOs;
using Assignment__Management_System.Factories;
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

        public ResponseModel<IQueryable<CourseEnrollDTO>> GetCourseEnrollments(string studentid)
        {
            var CourseEnrolls = context.CourseEnrollments.AsNoTracking()
                .Include(e => e.course)
                .Where(e => e.StuId == studentid)
                .Select(x => new CourseEnrollDTO {
                    CrsId = x.CrsId,
                    CrsName = x.course.CrsName
                });

            if (CourseEnrolls.Any())
                return new ResponseModelFactory()
                    .CreateResponseModel<IQueryable<CourseEnrollDTO>>(true, "", CourseEnrolls);
            else
                return new ResponseModelFactory()
                    .CreateResponseModel<IQueryable<CourseEnrollDTO>>(false, "No Available Courses!", null);
        }

        public ResponseModel<IQueryable<AssignmentSubsDetails>> GetAssignmentDetails(int assignid, string studid)
        {
            var Assignsub = context.Submissions.AsNoTracking()
                .Include(e => e.assignment)
                .Where(s => s.AssignmentId == assignid && s.StuId == studid)
                .Select(x => new AssignmentSubsDetails()
                {
                    Title = x.assignment.Title,
                    DeadLine = x.assignment.DeadLine,
                    FileName = Path.GetFileName(x.FilePath),
                    grade = x.grade ?? 0
                });

            if (Assignsub.Any())
                return new ResponseModelFactory()
                    .CreateResponseModel<IQueryable<AssignmentSubsDetails>>(true, "", Assignsub);
            else
                return new ResponseModelFactory()
                    .CreateResponseModel<IQueryable<AssignmentSubsDetails>>(false, "No Available Submissions!", null);
        }
    }
}
