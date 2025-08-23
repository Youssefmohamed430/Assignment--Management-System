using Assignment__Management_System.Models.Entities;
using Assignment__Management_System.Services;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Student")]
        [HttpGet("SubmitDetails")]
        public IActionResult GetsubmissionDetails([FromQuery]int assignid)
        {
            var studid = User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;

            var result = studentService.GetAssignmentDetails(assignid, studid);

            return result.IsSuccess ? Ok(result) : BadRequest(result); 
        }
        [Authorize(Roles = "Student")]
        [HttpGet]
        public IActionResult GetCourseEnrollments()
        {
            var studentid = User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;

            var result = studentService.GetCourseEnrollments(studentid);

            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
        [Authorize(Roles = "Instructor")]
        [HttpGet("{name}")]
        public IActionResult GetStudentByname(string name)
        {
            var result = studentService.GetStudentByname(name);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
    }
}
