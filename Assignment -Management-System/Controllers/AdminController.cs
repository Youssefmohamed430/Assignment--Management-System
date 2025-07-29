using Assignment__Management_System.DataLayer.DTOs;
using Assignment__Management_System.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assignment__Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService Iadminservice;
        public AdminController(IAdminService _Iadminservice)
        { 
            this.Iadminservice = _Iadminservice;
        }

        [HttpPost("Addcourse")]
        public IActionResult AddNewCourse(CourseDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = Iadminservice.AddCourses(model);

            return result == "" ? Ok() : BadRequest(result); 
        }
    }
}
