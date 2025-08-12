using Assignment__Management_System.DataLayer.DTOs;
using Assignment__Management_System.Models.Data;
using Assignment__Management_System.Models.Entities;

namespace Assignment__Management_System.Services
{
    public class AdminServices : IAdminService
    {
        private readonly AppDbContext _context;

        public AdminServices(AppDbContext context)
        {
            _context = context;
        }
    }
}
