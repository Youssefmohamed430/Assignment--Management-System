using Assignment__Management_System.Models.Entities;

namespace Assignment__Management_System.Services
{
    public interface IInstructorService
    {
        string AddAssignmentToCourse(string userid, Assignment assignment);
    }
}
