using Assignment__Management_System.DataLayer;
using Assignment__Management_System.DataLayer.DTOs;
using Assignment__Management_System.Models.Entities;
using Azure;

namespace Assignment__Management_System.Services
{
    public interface ICourseService
    {
        ResponseModel<CourseDto> AddCourses(CourseDto model);
        ResponseModel<CourseEnrollDTO> EnrollCourse(CourseEnrollDTO model,string userid);
        ResponseModel<IQueryable<CourseDto>> GetCourses();
        ResponseModel<CourseDto> UpdateCourses(CourseDto model,int crsid);
        ResponseModel<CourseDto> DeleteCourses(int crsid);
    }
}
