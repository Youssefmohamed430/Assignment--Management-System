using Assignment__Management_System.DataLayer;
using Assignment__Management_System.DataLayer.DTOs;
using Assignment__Management_System.Factories;
using Assignment__Management_System.Models;
using Assignment__Management_System.Models.Data;
using Assignment__Management_System.Models.Entities;
using Azure.Core;
using Microsoft.EntityFrameworkCore;

namespace Assignment__Management_System.Services
{
    public class InstructorService : IInstructorService
    {
        private readonly AppDbContext _context;
        private readonly TokenRequestModel Request;
        private readonly INotificationService _notificationService;

        public InstructorService(AppDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }
        public ResponseModel<AssignmentDTO> AddAssignmentToCourse(string userid, AssignmentDTO model)
        {
            var Inst = _context.Instructors
                .Include(i => i.Courses)
                .Include(i => i.User)
                .FirstOrDefault(i => i.Id == userid);

            var assignment = new Assignment()
            {
                Title = model.Title,
                DeadLine = model.DeadLine,
                CrsId = model.CrsId,
            };

            _context.Assignments.Add(assignment);

            try
            {
                _context.SaveChanges();
                
                _notificationService.NotifyStudentsOfNewAssignment(assignment);

                return new ResponseModelFactory()
                    .CreateResponseModel<AssignmentDTO>(true,"Adding Successfully",model);
            }
            catch (Exception ex)
            {
                return new ResponseModelFactory()
                    .CreateResponseModel<AssignmentDTO>(false, ex.Message, null);
            }
        }
        public ResponseModel<AssignmentDTO> UpdateAssignmentsGrades(int submissionId,double Grade)
        {
            try
            {
                var sub = _context.Submissions.AsNoTracking()
                .FirstOrDefault(s => s.SubId == submissionId);

                sub.grade = Grade;

                _context.Update(sub);

                _context.SaveChanges();

                return new ResponseModelFactory()
                     .CreateResponseModel<AssignmentDTO>(true,"Updat Grades success", null);
            }
            catch(Exception ex)
            {
                return new ResponseModelFactory()
                     .CreateResponseModel<AssignmentDTO>(false, ex.Message, null);
            }
        }
        public ResponseModel<IQueryable<Submission>>  GetSubmissions(int AssignId)
        {
            var subs = _context.Submissions.AsNoTracking()
                .Include(s => s.assignment)
                .Include(s => s.student)
                .ThenInclude(s => s.User)
                .Where(s => s.AssignmentId == AssignId);

            if (subs != null)
                return new ResponseModelFactory()
                    .CreateResponseModel<IQueryable<Submission>>(true, "", subs);
            else
                return new ResponseModelFactory()
                    .CreateResponseModel<IQueryable<Submission>>(false, "No available submissions!", null);
        }

        public ResponseModel<IQueryable<AssignmentStudentGrades>> GetAssignmentStudentGrades(int assignmentid)
        {
            var studgrades = _context.Submissions
                .AsNoTracking()
                .Include(s => s.student)
                .ThenInclude(s => s.User)
                .Where(s => s.AssignmentId == assignmentid)
                .Select(s => new AssignmentStudentGrades()
                {
                    StudentName = s.student.User.Name,
                    Grade = s.grade,
                });

            if (studgrades != null)
                return new ResponseModelFactory()
                  .CreateResponseModel<IQueryable<AssignmentStudentGrades>>(true, "", studgrades);
            else
                return new ResponseModelFactory()
                  .CreateResponseModel<IQueryable<AssignmentStudentGrades>>(false, "No Submission available for this assignment!", null);
        }

        public ResponseModel<IQueryable<InstructorDTO>> GetInstructors()
        {
            var insts = _context.Instructors
                .AsNoTracking()
                .Include(i => i.User)
                .Select(i => new InstructorDTO()
                {
                    Name = i.User.Name,
                    id = i.Id
                });

            if (insts != null)
                return new ResponseModelFactory()
                    .CreateResponseModel<IQueryable<InstructorDTO>>(true, "", insts);
            else
                return new ResponseModelFactory()
                    .CreateResponseModel<IQueryable<InstructorDTO>>(false, "No Instructors available!", null);
        }
        public ResponseModel<IQueryable<CourseDto>> GetInstructorCourses(string instid)
        {
            var course = _context.Courses
                            .AsNoTracking()
                            .Include(c => c.instructor)
                            .ThenInclude(c => c.User)
                            .Where(c => c.InstId == instid)
                            .Select(x => new CourseDto()
                            {
                                Id = x.CrsId,
                                CrsName = x.CrsName,
                                InstId = x.InstId,
                                InstName = x.instructor.User.Name
                            });

            if (course == null)
                return new ResponseModelFactory()
                 .CreateResponseModel<IQueryable<CourseDto>>(false, "No Available Courses!", null);
            else
                return new ResponseModelFactory()
                 .CreateResponseModel<IQueryable<CourseDto>>(true, "", course);
        }
    }
}
