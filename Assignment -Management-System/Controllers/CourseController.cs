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

        [HttpGet]
        public IActionResult GetCourses()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = courseService.GetCourses();

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public IActionResult AddNewCourse(CourseDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = courseService.AddCourses(model);

            return result.IsSuccess ? Created() : BadRequest(result);
        }

        [HttpPost("EnrollCourse")]
        public IActionResult EnrollCourse(CourseEnrollDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;

            var result = courseService.EnrollCourse(model,userId);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        

        [HttpPut("{id}")]
        public IActionResult UpdateCourse([FromBody] CourseDto crs,int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = courseService.UpdateCourses(crs, id);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAssignment(int id)
        {
            var result = courseService.DeleteCourses(id);

            return result.IsSuccess == true ? Ok(result) : BadRequest(result);
        }

    }
}
