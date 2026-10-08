using System;
using System.Collections.Generic;

namespace Assignment__Management_System.DataLayer.DTOs
{
    public class InstructorRecentActivityDto
    {
        public string ActivityType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }

    public class InstructorAssignmentSubmissionsDto
    {
        public int AssignmentId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public int PendingSubmissions { get; set; }
    }

    public class InstructorDashboardDto
    {
        public int MyCourses { get; set; }
        public int TotalStudents { get; set; }
        public int PendingSubmissions { get; set; }
        public int TotalAssignments { get; set; }
        public double AverageCourseGrade { get; set; }
        public int LateSubmissions { get; set; }
        public List<InstructorRecentActivityDto> RecentActivity { get; set; } = new();
        public List<InstructorAssignmentSubmissionsDto> SubmissionAssignments { get; set; } = new();
    }
}
