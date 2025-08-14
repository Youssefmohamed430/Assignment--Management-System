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
        public IQueryable<Assignment> GetAssignments(int CrsId)
        {
            var assignments = _context.Assignments.AsNoTracking()
                .Where(a => a.CrsId == CrsId);

            return assignments;
        }
    }
}
