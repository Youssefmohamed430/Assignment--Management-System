using Assignment__Management_System.DataLayer.DTOs;
using Assignment__Management_System.Models;
using Assignment__Management_System.Services;
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
        public async Task<IActionResult> LoginAsync(TokenRequestModel Tokenmodel)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(Tokenmodel);

            if(!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUserAsync(UserDto model)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.AddUserAsync(model);

            if(!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }
    }
}
