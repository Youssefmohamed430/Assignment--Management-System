using Assignment__Management_System.DataLayer.DTOs;
using Assignment__Management_System.Models.Entities;
using Assignment__Management_System.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [HttpPost]
        public IActionResult SubmitAssignment(SubmitDTO sub)
        {
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            var result = Submissionservice.SubmitAssignment(sub);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpGet("{assignid}")]
        public IActionResult GetSubs(int assignid)
        {
            var result = Submissionservice.GetSubs(assignid);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
