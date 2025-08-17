using Assignment__Management_System.DataLayer.DTOs;
using Assignment__Management_System.Models.Entities;

namespace Assignment__Management_System.Services
{
    public interface ISubmissionService
    {
        string SubmitAssignment(SubmitDTO Sub);
        IQueryable<SubmitDTO> GetSubs(int assignid);
    }
}
