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
            var result = assignmentService.GetAssignments(crsid).ToList();

            return result != null ? Ok(result) : BadRequest("No Assignments!");
        }
        
    }
}
