using Assignment__Management_System.Models.Entities;

namespace Assignment__Management_System.Services
{
    public interface IAssignmentService
    {
        IQueryable<Assignment> GetAssignments(int CrsId);
    }
}
