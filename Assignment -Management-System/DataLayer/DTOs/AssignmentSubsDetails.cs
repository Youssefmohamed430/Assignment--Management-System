using Assignment__Management_System.Models.Entities;

namespace Assignment__Management_System.DataLayer.DTOs
{
    public class AssignmentSubsDetails
    {
        public string Title { get; set; }
        public DateOnly DeadLine { get; set; }
        public string FileName { get; set; }
        public double? grade { get; set; }
    }
}
