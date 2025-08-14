using Assignment__Management_System.Models.Entities;
using Assignment__Management_System.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Assignment__Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : Controller
    {
        private readonly INotificationService notificationService;

        public NotificationController(INotificationService notificationService)
        {
            this.notificationService = notificationService;
        }

        [HttpGet("GetNotifications")]
        public IActionResult GetNotifications()
        {
            var userid = User.FindFirstValue("uid");

            var result = notificationService.GetNotifications(userid);

            return result == null ? Ok(result) : BadRequest("No new notifications");
        }
        [HttpPut("MarkasRead")]
        public void MarkasRead()
        {
            throw new NotImplementedException();
        }
    }
}
