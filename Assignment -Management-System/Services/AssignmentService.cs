using Assignment__Management_System.DataLayer;
using Assignment__Management_System.DataLayer.DTOs;
using Assignment__Management_System.Factories;
using Assignment__Management_System.Models.Data;
using Assignment__Management_System.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Assignment__Management_System.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly AppDbContext _context;

        public AssignmentService(AppDbContext context)
        {
            _context = context;
        }

        public ResponseModel<AssignmentDTO> DeleteAssignment(int assignmentid)
        {
            try
            {
                var assignment = _context.Assignments.FirstOrDefault(a => a.Id == assignmentid);

                _context.Assignments.Remove(assignment);

                _context.SaveChanges();

                return new ResponseModelFactory()
                    .CreateResponseModel<AssignmentDTO>(true, "Deleted Successfully!", null);
            }
            catch (Exception ex)
            {
                return new ResponseModelFactory()
                    .CreateResponseModel<AssignmentDTO>(false, ex.Message, null);
            }
        }

        public ResponseModel<IQueryable<AssignmentDTO>> GetAssignments(int CrsId)
        {
            var assignments = _context.Assignments.AsNoTracking()
                .Where(a => a.CrsId == CrsId)
                .Select(x => new AssignmentDTO()
                {
                    AssignmentId = x.Id,
                    Title = x.Title,
                    DeadLine = x.DeadLine,
                    CrsId = CrsId
                });

            if (assignments.Any())
                return new ResponseModelFactory()
                    .CreateResponseModel<IQueryable<AssignmentDTO>>(true, "", assignments);
            else
                return new ResponseModelFactory()
                    .CreateResponseModel<IQueryable<AssignmentDTO>>(false, "No Assignments For this Course!", null);
        }
        public ResponseModel<AssignmentDTO> GetAssignmentById(int assignmentid)
        {
            var assignment = _context.Assignments.AsNoTracking()
                .Where(a => a.Id == assignmentid)
                .Select(x => new AssignmentDTO()
                {
                    Title = x.Title,
                    DeadLine = x.DeadLine,
                    CrsId = x.CrsId
                }).FirstOrDefault();

            if (assignment != null)
                return new ResponseModelFactory()
                    .CreateResponseModel<AssignmentDTO>(true, "", assignment);
            else
                return new ResponseModelFactory()
                    .CreateResponseModel<AssignmentDTO>(false, "Assignment Not Found!", null);
        }
        public ResponseModel<AssignmentDTO> UpdateAssignment(AssignmentDTO assignment,int id)
        {
            try
            {
                var oldAssignment = _context.Assignments.FirstOrDefault(a => a.Id == id);

                oldAssignment.Title = assignment.Title;
                oldAssignment.DeadLine = assignment.DeadLine;

                _context.Assignments.Update(oldAssignment);

                _context.SaveChanges();

                return new ResponseModelFactory()
                    .CreateResponseModel<AssignmentDTO>(true, "Updated Successfully!", assignment);
            }
            catch (Exception ex)
            {
                return new ResponseModelFactory()
                    .CreateResponseModel<AssignmentDTO>(false, ex.Message, null);
            }
        }
    }
}
