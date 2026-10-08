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
                if (assignment == null)
                    return new ResponseModelFactory()
                        .CreateResponseModel<AssignmentDTO>(false, "Assignment not found!", null);

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

        public ResponseModel<IQueryable<AssignmentDTO>> GetAssignments(int CrsId, bool includeHidden = false)
        {
            var assignments = _context.Assignments.AsNoTracking()
                .Include(a => a.course)
                .Where(a => a.CrsId == CrsId &&
                    (includeHidden || (a.Status != AssignmentStatus.Draft && a.Status != AssignmentStatus.Archived)))
                .Select(x => new AssignmentDTO()
                {
                    AssignmentId = x.Id,
                    Title = x.Title,
                    DeadLine = x.DeadLine,
                    CrsId = CrsId,
                    CrsName = x.course != null ? x.course.CrsName : "",
                    Status = x.Status,
                    PublishedAt = x.PublishedAt,
                    ClosedAt = x.ClosedAt
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
                .Include(a => a.course)
                .Where(a => a.Id == assignmentid)
                .Select(x => new AssignmentDTO()
                {
                    AssignmentId = assignmentid,
                    Title = x.Title,
                    DeadLine = x.DeadLine,
                    CrsId = x.CrsId,
                    CrsName = x.course != null ? x.course.CrsName : "",
                    Status = x.Status,
                    PublishedAt = x.PublishedAt,
                    ClosedAt = x.ClosedAt
                }).FirstOrDefault();

            if (assignment != null)
                return new ResponseModelFactory()
                    .CreateResponseModel<AssignmentDTO>(true, "", assignment);
            else
                return new ResponseModelFactory()
                    .CreateResponseModel<AssignmentDTO>(false, "Assignment Not Found!", null);
        }

        public ResponseModel<AssignmentDTO> UpdateAssignment(AssignmentDTO assignment, int id)
        {
            try
            {
                var oldAssignment = _context.Assignments.Include(a => a.course).FirstOrDefault(a => a.Id == id);
                if (oldAssignment == null)
                    return new ResponseModelFactory()
                        .CreateResponseModel<AssignmentDTO>(false, "Assignment not found!", null);

                oldAssignment.Title = assignment.Title;
                oldAssignment.DeadLine = assignment.DeadLine;
                if (assignment.Status.HasValue)
                {
                    oldAssignment.Status = assignment.Status.Value;
                }

                _context.Assignments.Update(oldAssignment);
                _context.SaveChanges();

                var assigndto = new AssignmentDTO()
                {
                    AssignmentId = oldAssignment.Id,
                    Title = oldAssignment.Title,
                    DeadLine = oldAssignment.DeadLine,
                    CrsId = oldAssignment.CrsId,
                    CrsName = oldAssignment.course != null ? oldAssignment.course.CrsName : "",
                    Status = oldAssignment.Status,
                    PublishedAt = oldAssignment.PublishedAt,
                    ClosedAt = oldAssignment.ClosedAt
                };

                return new ResponseModelFactory()
                    .CreateResponseModel<AssignmentDTO>(true, "Updated Successfully!", assigndto);
            }
            catch (Exception ex)
            {
                return new ResponseModelFactory()
                    .CreateResponseModel<AssignmentDTO>(false, ex.Message, null);
            }
        }
    }
}
