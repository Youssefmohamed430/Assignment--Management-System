using Assignment__Management_System.DataLayer.DTOs;
using Assignment__Management_System.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("EnrollCourse")]
        public IActionResult EnrollCourse(CourseEnrollDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = courseService.EnrollCourse(model);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("GetCourses")]
        public IActionResult GetCourses() //بتاعت ال Instructor
        {
            var instid = User.FindFirstValue("uid");

            var result = courseService.GetCourses(instid);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("Update")]
        public IActionResult UpdateCourse([FromBody] CourseDto crs, [FromQuery] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = courseService.UpdateCourses(crs, id);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("Delete")]
        public IActionResult DeleteAssignment([FromQuery] int id)
        {
            var result = courseService.DeleteCourses(id);

            return result.IsSuccess == true ? Ok(result) : BadRequest(result);
        }

    }
}
