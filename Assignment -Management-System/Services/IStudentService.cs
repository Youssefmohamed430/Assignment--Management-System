using Assignment__Management_System.DataLayer.DTOs;
using Assignment__Management_System.Models.Entities;

namespace Assignment__Management_System.Services
{
    public interface IStudentService
    {
        IQueryable<CourseEnrollDTO> GetCourseEnrollments(string studentid);
        IQueryable<AssignmentSubsDetails> GetAssignmentDetails(int assignid, string studid);
    }
}
