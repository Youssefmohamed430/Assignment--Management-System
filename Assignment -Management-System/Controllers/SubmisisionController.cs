using Assignment__Management_System.DataLayer.DTOs;
using Assignment__Management_System.Models.Entities;
using Assignment__Management_System.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Assignment__Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubmisisionController : Controller
    {
        private readonly ISubmissionService Submissionservice;

        public SubmisisionController(ISubmissionService submissionservice)
        {
            Submissionservice = submissionservice;
        }

        [Authorize(Roles = "Student")]
        [HttpPost]
        [RequestSizeLimit(10 * 1024 * 1024)]
        public IActionResult SubmitAssignment([FromForm] SubmitDTO sub)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var studentid = User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;

            if (string.IsNullOrWhiteSpace(studentid))
                return Unauthorized();

            var result = Submissionservice.SubmitAssignment(sub, studentid);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Instructor")]
        [HttpGet("{assignid}")]
        public IActionResult GetSubs(int assignid)
        {
            var result = Submissionservice.GetSubs(assignid);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [Authorize(Roles = "Instructor")]
        [HttpGet("file/{submissionId}")]
        public IActionResult GetSubmissionFile(int submissionId)
        {
            var result = Submissionservice.GetSubmissionFile(submissionId);

            if (!result.IsSuccess)
                return NotFound(result);

            return File(
                result.Result.FileBytes,
                result.Result.ContentType,
                result.Result.FileName);
        }
    }
}
