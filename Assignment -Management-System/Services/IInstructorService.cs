using Assignment__Management_System.DataLayer;
using Assignment__Management_System.DataLayer.DTOs;
using Assignment__Management_System.Models.Entities;

namespace Assignment__Management_System.Services
{
    public interface IInstructorService
    {
        ResponseModel<AssignmentDTO> AddAssignmentToCourse(string userid, AssignmentDTO assignment);
        ResponseModel<IQueryable<Submission>> GetSubmissions(int AssignId);
        ResponseModel<AssignmentDTO> UpdateAssignmentsGrades(int submissionId, double Grade);
        ResponseModel<IQueryable<AssignmentStudentGrades>> GetAssignmentStudentGrades(int assignmentid);
        ResponseModel<IQueryable<InstructorDTO>> GetInstructors();
        ResponseModel<IQueryable<CourseDto>> GetInstructorCourses(string instid);
    }
}
