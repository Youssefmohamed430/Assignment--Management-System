using Assignment__Management_System.Models.Entities;
using Assignment__Management_System.Services;
using Microsoft.AspNetCore.Mvc;

namespace Assignment__Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : Controller
    {
        private IStudentService studentService;

        public StudentController(IStudentService studentService)
        {
            this.studentService = studentService;
        }
        [HttpGet("GetsubmissionDetails")]
        public IActionResult GetsubmissionDetails(int assignid, string studid)
        {
            var result = studentService.GetAssignmentDetails(assignid, studid);

            return result.IsSuccess ? Ok(result) : BadRequest(result); 
        }
        [HttpGet("GetCourseEnrollments")]
        public IActionResult GetCourseEnrollments(string studentid)
        {
            var result = studentService.GetCourseEnrollments(studentid);

            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
    }
}
