using Assignment__Management_System.Models.Entities;
using Assignment__Management_System.Services;
using Assignment__Management_System.DataLayer.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Assignment__Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : Controller
    {
        private readonly IStudentService studentService;

        public StudentController(IStudentService studentService)
        {
            this.studentService = studentService;
        }

        [Authorize(Roles = "Student")]
        [HttpGet("dashboard")]
        public IActionResult GetDashboard()
        {
            var studentId = User.FindFirstValue("uid") ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(studentId))
                return Unauthorized();

            var result = studentService.GetDashboard(studentId);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Student")]
        [HttpGet("SubmitDetails")]
        public IActionResult GetsubmissionDetails([FromQuery] int assignid)
        {
            var studid = User.FindFirstValue("uid") ?? User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = studentService.GetAssignmentDetails(assignid, studid);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Student")]
        [HttpGet]
        public IActionResult GetCourseEnrollments()
        {
            var studentid = User.FindFirstValue("uid") ?? User.FindFirstValue(ClaimTypes.NameIdentifier);

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

        [Authorize(Roles = "Student")]
        [HttpPut("ProfileImage")]
        [RequestSizeLimit(ImageStorageService.MaxImageSize)]
        public IActionResult UpdateProfileImage([FromForm] ImageUploadDTO model)
        {
            var studentId = User.FindFirstValue("uid") ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = studentService.UpdateProfileImage(studentId, model.Image);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [Authorize]
        [HttpGet("ProfileImage/{studentId}")]
        public IActionResult GetProfileImage(string studentId)
        {
            var result = studentService.GetProfileImage(studentId);
            if (!result.IsSuccess)
                return NotFound(result);

            return File(result.Result.FileBytes, result.Result.ContentType);
        }
    }
}
