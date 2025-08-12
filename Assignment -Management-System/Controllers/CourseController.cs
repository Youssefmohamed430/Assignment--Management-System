using Assignment__Management_System.DataLayer.DTOs;
using Assignment__Management_System.Services;
using Microsoft.AspNetCore.Mvc;

namespace Assignment__Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : Controller
    {
        private readonly ICourseService courseService;

        public CourseController(ICourseService _courseService)
        {
            this.courseService = _courseService;
        }

        [HttpPost("Addcourse")]
        public IActionResult AddNewCourse(CourseDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = courseService.AddCourses(model);

            return result == "" ? Ok() : BadRequest(result);
        }

        [HttpPost("EnrollCourse")]
        public IActionResult EnrollCourse(CourseEnrollDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = courseService.EnrollCourse(model);

            return result == "" ? Ok() : BadRequest(result);
        }

    }
}
