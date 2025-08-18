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

        [HttpGet("GetNotifications")]
        public IActionResult GetNotifications(string stuid)
        {
            var result = notificationService.GetNotifications(stuid);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpPut("MarkasRead")]
        public IActionResult MarkasRead(string stuid,int notifid)
        {
            var result = notificationService.MarkasRead(stuid, notifid);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
