using System;
using Assignment__Management_System.Models.Entities;
using Microsoft.AspNetCore.Http;

namespace Assignment__Management_System.DataLayer.DTOs
{
    public class AssignmentDTO
    {
        public int? AssignmentId { get; set; }
        public string Title { get; set; }
        public string? CrsName { get; set; }
        public int? CrsId { get; set; }
        public DateTime DeadLine { get; set; }
        public IFormFile? File { get; set; }
        public string? FileName { get; set; }
        public AssignmentStatus? Status { get; set; }
        public DateTime? PublishedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
    }
}
