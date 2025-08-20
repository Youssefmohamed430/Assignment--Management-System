using Assignment__Management_System.Models.Entities;
using Assignment__Management_System.Services;
using Microsoft.AspNetCore.Mvc;
using System;
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

        [HttpGet]
        public IActionResult GetNotifications()
        {
            var stuid = User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;

            var result = notificationService.GetNotifications(stuid);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpPut("MarkasRead/{notifid}")]
        public IActionResult MarkasRead(int notifid)
        {
            var stuid = User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;

            var result = notificationService.MarkasRead(stuid, notifid);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
