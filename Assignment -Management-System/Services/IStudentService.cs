using Assignment__Management_System.DataLayer;
using Assignment__Management_System.DataLayer.DTOs;
using Assignment__Management_System.Models.Entities;

namespace Assignment__Management_System.Services
{
    public interface IStudentService
    {
        ResponseModel<IQueryable<CourseEnrollDTO>> GetCourseEnrollments(string studentid);
        ResponseModel<IQueryable<AssignmentSubsDetails>> GetAssignmentDetails(int assignid, string studid);
        ResponseModel<UserDto> GetStudentByname(string name);
    }
}
