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
        [HttpGet("{crsid}")]
        public IActionResult GetAssignments(int crsid)
        {
            var result = assignmentService.GetAssignments(crsid);

            return result != null ? Ok(result) : BadRequest("No Assignments!");
        }
        [HttpPut("{id}")]
        public IActionResult UpdateAssignment([FromBody] AssignmentDTO assignment,int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = assignmentService.UpdateAssignment(assignment, id);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAssignment(int id)
        {
            var result = assignmentService.DeleteAssignment(id);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("ById/{id}")]
        public IActionResult GetAssignmentById(int id)
        {
            var result = assignmentService.GetAssignmentById(id);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
