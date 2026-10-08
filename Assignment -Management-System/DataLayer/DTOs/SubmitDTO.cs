using Assignment__Management_System.Models.Entities;
using Microsoft.AspNetCore.Http;

namespace Assignment__Management_System.DataLayer.DTOs
{
    public class SubmitDTO
    {
        public IFormFile? File { get; set; }
        public string? FileName { get; set; }
        public int AssignmentId { get; set; }
        public string? AssignmentTitle { get; set; }
        public double? grade { get; set; }
        public string? stuname { get; set; }
        public int? SubmissionId { get; set; }
        public int AttemptNumber { get; set; }
        public DateTime SubmitedAt { get; set; } = DateTime.Now;
        public string? Feedback { get; set; } = "";
        public bool IsLate { get; set; } = false;
        public SubmissionStatus Status { get; set; } = SubmissionStatus.Pending;
    }
}
