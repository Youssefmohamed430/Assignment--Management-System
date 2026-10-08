namespace Assignment__Management_System.Models.Entities
{
    public enum SubmissionStatus
    {
        Pending,
        Graded,
        Late,
        Resubmitted
    }
    public class Submission
    {
        public int SubId { get; set; }
        public string FilePath { get; set; }
        public double? grade { get; set; }
        public string StuId { get; set; }
        public int AssignmentId { get; set; }
        public int AttemptNumber { get; set; } = 1;
        public DateTime SubmitedAt { get; set; } = DateTime.Now;
        public string? Feedback { get; set; } = "";
        public bool IsLate { get; set; } = false;
        public SubmissionStatus Status { get; set; } = SubmissionStatus.Pending;
        public Assignment? assignment { get; set; }
        public Student? student { get; set; }
    }
}
