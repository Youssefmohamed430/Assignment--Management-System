using Assignment__Management_System.DataLayer.DTOs;
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
        public string AddAssignmentToCourse(string userid, AssignmentDTO model)
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
                CrsId = model.CrsId,
            };

            _context.Assignments.Add(assignment);

            try
            {
                _context.SaveChanges();
                Message = "";
                return Message;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
            Message = "Something went wrong while adding the assignment to the course!";
            return Message;
        }
        public string UpdateAssignmentsGrades(int submissionId,double Grade)
        {
            try
            {
                var sub = _context.Submissions.AsNoTracking()
                .FirstOrDefault(s => s.SubId == submissionId);

                sub.grade = Grade;

                _context.Update(sub);

                _context.SaveChanges();

                return "";
            }
                catch(Exception ex)
            {
                return ex.Message;
            }
        }
        public IQueryable<Submission> GetSubmissions(int AssignId)
        {
            var subs = _context.Submissions.AsNoTracking()
                .Include(s => s.assignment)
                .Include(s => s.student)
                .ThenInclude(s => s.User)
                .Where(s => s.AssignmentId == AssignId);

            return subs;
        }
    }
}
