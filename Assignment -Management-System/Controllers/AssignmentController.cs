using Assignment__Management_System.DataLayer.DTOs;
using Assignment__Management_System.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Assignment__Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentController : Controller
    {
        private readonly IAssignmentService assignmentService;

        public AssignmentController(IAssignmentService assignmentService)
        {
            this.assignmentService = assignmentService;
        }
        [HttpGet("GetAssignments")]
        public IActionResult GetAssignments(int crsid)
        {
            var result = assignmentService.GetAssignments(crsid);

            return result != null ? Ok(result) : BadRequest("No Assignments!");
        }
        [HttpPost("Update")]
        public IActionResult UpdateAssignment([FromBody] AssignmentDTO assignment, [FromQuery] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = assignmentService.UpdateAssignment(assignment, id);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("Delete")]
        public IActionResult DeleteAssignment([FromQuery]int id)
        {
            var result = assignmentService.DeleteAssignment(id);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
