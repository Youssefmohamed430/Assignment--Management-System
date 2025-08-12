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

        public InstructorService(AppDbContext context)
        {
            _context = context;
        }
        public string AddAssignmentToCourse(string userid, Assignment model)
        {
            string Message = "";

            var Inst = _context.Instructors
                .Include(i => i.Courses)
                .Include(i => i.User)
                .FirstOrDefault(i => i.Id == userid);

            var assignment = new Assignment()
            {
                Title = model.Title,
                DeadLine = model.DeadLine,
                CrsId = model.Id
            };

            _context.Assignments.Add(assignment);

            try
            {
                _context.SaveChanges();
                return Message;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
            Message = "Something went wrong while adding the assignment to the course!";
            return Message;
        }
        public string UpdateAssignmentsGrades(List<Submission> submissions)
        {
            foreach (var submission in submissions) 
                _context.Submissions.Update(submission);

            _context.SaveChanges();

            return "";
        }

    }
}
