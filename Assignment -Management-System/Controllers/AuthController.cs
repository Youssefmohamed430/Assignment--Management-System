using Assignment__Management_System.DataLayer.DTOs;
using Assignment__Management_System.Models;
using Assignment__Management_System.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assignment__Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody]TokenRequestModel Tokenmodel)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(Tokenmodel);

            if(!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RefreshTokenAsync(request.RefreshToken);
            return result.IsAuthenticated ? Ok(result) : Unauthorized(result);
        }

        [AllowAnonymous]
        [HttpPost("RevokeToken")]
        public async Task<IActionResult> RevokeTokenAsync([FromBody] RefreshTokenRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _authService.RevokeRefreshTokenAsync(request.RefreshToken);
            return NoContent();
        }
        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterInstructorAsync(UserDto model)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            model.Role = "Instructor";
            var result = await _authService.RegisterUserAsync(model);

            if(!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("RegisterStudent")]
        public async Task<IActionResult> RegisterStudentAsync(UserDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            model.Role = "Student"; 

            var result = await _authService.RegisterUserAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }
    }
}
