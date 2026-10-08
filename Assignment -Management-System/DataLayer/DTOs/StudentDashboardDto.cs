using System;
using System.Collections.Generic;

namespace Assignment__Management_System.DataLayer.DTOs
{
    public class UpcomingAssignmentDto
    {
        public int AssignmentId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public DateTime Deadline { get; set; }
        public string RemainingTime { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public class RecentGradeDto
    {
        public int AssignmentId { get; set; }
        public string AssignmentTitle { get; set; } = string.Empty;
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public double Grade { get; set; }
        public DateTime? GradedAt { get; set; }
    }

    public class StudentDashboardDto
    {
        public int MyCourses { get; set; }
        public int PendingAssignments { get; set; }
        public int DueTomorrow { get; set; }
        public double AverageGrade { get; set; }
        public List<UpcomingAssignmentDto> UpcomingDeadlines { get; set; } = new();
        public List<RecentGradeDto> RecentGrades { get; set; } = new();
    }
}
