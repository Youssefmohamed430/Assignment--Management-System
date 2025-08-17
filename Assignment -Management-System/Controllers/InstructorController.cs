using Assignment__Management_System.DataLayer.DTOs;
using Assignment__Management_System.Models.Entities;
using Assignment__Management_System.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Assignment__Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : Controller
    {
        private IInstructorService _instructorService;
        public InstructorController(IInstructorService instructorService)
        {
            _instructorService = instructorService;
        }
        [HttpPost("AddAssignment")]
        public IActionResult AddAssignmentToCourse(AssignmentDTO assignment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);  

            var userid = User.FindFirstValue("uid");
            
            var result = _instructorService.AddAssignmentToCourse(userid, assignment);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpPost("UpdateAssignmentsGrades")]
        public IActionResult UpdateAssignmentsGrades(int Subid , double grade)
        {
            var result = _instructorService.UpdateAssignmentsGrades(Subid,grade);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
