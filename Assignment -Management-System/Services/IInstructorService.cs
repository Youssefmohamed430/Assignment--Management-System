using Assignment__Management_System.DataLayer.DTOs;
using Assignment__Management_System.Models.Entities;

namespace Assignment__Management_System.Services
{
    public interface IInstructorService
    {
        string AddAssignmentToCourse(string userid, AssignmentDTO assignment);
        string UpdateAssignmentsGrades(List<Submission> submissions);
    }
}
