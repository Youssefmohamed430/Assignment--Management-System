using Assignment__Management_System.DataLayer.DTOs;
using Assignment__Management_System.Models.Entities;
using Assignment__Management_System.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Assignment__Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InstructorController : Controller
    {
        private IInstructorService _instructorService;
        public InstructorController(IInstructorService instructorService)
        {
            _instructorService = instructorService;
        }

        [Authorize(Roles = "Instructor")]
        [HttpGet("InstructorCourses")]
        public IActionResult GetInstructorCourses()
        {
            var instid = User.FindFirstValue("uid");
            var result = _instructorService.GetInstructorCourses(instid);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Instructor")]
        [HttpPost]
        [RequestSizeLimit(10 * 1024 * 1024)]
        public IActionResult AddAssignmentToCourse([FromForm] AssignmentDTO assignment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userid = User.FindFirstValue("uid");
            var result = _instructorService.AddAssignmentToCourse(userid, assignment);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Instructor")]
        [HttpGet("file/{assignmentId}")]
        public IActionResult GetAssignmentFile(int assignmentId)
        {
            var result = _instructorService.GetAssignmentFile(assignmentId);

            if (!result.IsSuccess)
                return NotFound(result);

            return File(
                result.Result.FileBytes,
                result.Result.ContentType,
                result.Result.FileName);
        }

        [Authorize(Roles = "Instructor")]
        [HttpPut]
        public IActionResult UpdateAssignmentsGrades([FromQuery]int Subid ,[FromQuery] double grade)
        {
            var result = _instructorService.UpdateAssignmentsGrades(Subid,grade);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Instructor")]
        [HttpGet("{assignmentid}")]
        public IActionResult GetAssignmentStudentGrades(int assignmentid)
        {
            var result = _instructorService.GetAssignmentStudentGrades(assignmentid);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetInstructors()
        {
            var result = _instructorService.GetInstructors();
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
