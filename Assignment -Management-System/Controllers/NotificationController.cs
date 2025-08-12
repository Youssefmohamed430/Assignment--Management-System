using Assignment__Management_System.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Assignment__Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : Controller
    {
        [HttpGet("GetNotifications")]
        public IQueryable<Notification> GetNotifications()
        {
            throw new NotImplementedException();
        }
        [HttpPut("MarkasRead")]
        public void MarkasRead()
        {
            throw new NotImplementedException();
        }
    }
}
