using System;
using System.Collections.Generic;

namespace Assignment__Management_System.Models.Entities
{
    public class Assignment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int CrsId { get; set; }
        public DateTime DeadLine { get; set; }
        public string? FilePath { get; set; }
        public AssignmentStatus Status { get; set; } = AssignmentStatus.Published;
        public DateTime? PublishedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public Course? course { get; set; }
        public List<Submission>? Submissions { get; set; }
    }
}
