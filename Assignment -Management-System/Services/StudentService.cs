using Assignment__Management_System.DataLayer;
using Assignment__Management_System.DataLayer.DTOs;
using Assignment__Management_System.Factories;
using Assignment__Management_System.Models.Data;
using Assignment__Management_System.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Assignment__Management_System.Services
{
    public class StudentService : IStudentService
    {
        private readonly AppDbContext context;
        private readonly ImageStorageService imageStorage;

        public StudentService(AppDbContext context, ImageStorageService imageStorage)
        {
            this.context = context;
            this.imageStorage = imageStorage;
        }

        public ResponseModel<IQueryable<CourseEnrollDTO>> GetCourseEnrollments(string studentid)
        {
            var CourseEnrolls = context.CourseEnrollments.AsNoTracking()
                .Include(e => e.course)
                .Where(e => e.StuId == studentid)
                .Select(x => new CourseEnrollDTO { CrsId = x.CrsId, CrsName = x.course.CrsName });

            return CourseEnrolls.Any()
                ? new ResponseModelFactory().CreateResponseModel<IQueryable<CourseEnrollDTO>>(true, "", CourseEnrolls)
                : new ResponseModelFactory().CreateResponseModel<IQueryable<CourseEnrollDTO>>(false, "No Available Courses!", null);
        }

        public ResponseModel<IQueryable<AssignmentSubsDetails>> GetAssignmentDetails(int assignid, string studid)
        {
            var Assignsub = context.Submissions.AsNoTracking()
                .Include(e => e.assignment)
                .Where(s => s.AssignmentId == assignid && s.StuId == studid)
                .Select(x => new AssignmentSubsDetails { Title = x.assignment.Title, DeadLine = x.assignment.DeadLine, FileName = Path.GetFileName(x.FilePath), grade = x.grade ?? 0 });

            return Assignsub.Any()
                ? new ResponseModelFactory().CreateResponseModel<IQueryable<AssignmentSubsDetails>>(true, "", Assignsub)
                : new ResponseModelFactory().CreateResponseModel<IQueryable<AssignmentSubsDetails>>(false, "No Available Submissions!", null);
        }

        public ResponseModel<UserDto> GetStudentByname(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return new ResponseModelFactory().CreateResponseModel<UserDto>(false, "Name cannot be empty!", null);

            var student = context.Users.AsNoTracking().Where(u => u.Name == name)
                .Select(s => new UserDto { UserName = s.UserName, Name = s.Name, Email = s.Email, ImageName = context.Students.Where(st => st.Id == s.Id).Select(st => st.ImagePath).FirstOrDefault() }).FirstOrDefault();

            return student != null
                ? new ResponseModelFactory().CreateResponseModel<UserDto>(true, "", student)
                : new ResponseModelFactory().CreateResponseModel<UserDto>(false, "Student not found!", null);
        }

        public ResponseModel<UserDto> UpdateProfileImage(string studentId, IFormFile image)
        {
            var student = context.Students.FirstOrDefault(s => s.Id == studentId);
            if (student == null)
                return new ResponseModelFactory().CreateResponseModel<UserDto>(false, "Student not found!", null);

            string? oldImage = student.ImagePath;
            string newImage;
            try
            {
                newImage = imageStorage.SaveImage(image, "Students");
                student.ImagePath = newImage;
                context.SaveChanges();
                imageStorage.DeleteImage("Students", oldImage);

                var user = context.Users.FirstOrDefault(u => u.Id == studentId);
                var dto = new UserDto { UserName = user?.UserName, Name = user?.Name, Email = user?.Email, ImageName = newImage };
                return new ResponseModelFactory().CreateResponseModel<UserDto>(true, "Profile image updated successfully!", dto);
            }
            catch (Exception ex)
            {
                return new ResponseModelFactory().CreateResponseModel<UserDto>(false, ex.Message, null);
            }
        }

        public ResponseModel<(byte[] FileBytes, string FileName, string ContentType)> GetProfileImage(string studentId)
        {
            var imageName = context.Students.AsNoTracking().Where(s => s.Id == studentId).Select(s => s.ImagePath).FirstOrDefault();
            if (string.IsNullOrWhiteSpace(imageName))
                return new ResponseModelFactory().CreateResponseModel<(byte[] FileBytes, string FileName, string ContentType)>(false, "No profile image found!", default);

            try
            {
                var image = imageStorage.ReadImage("Students", imageName);
                return new ResponseModelFactory().CreateResponseModel<(byte[] FileBytes, string FileName, string ContentType)>(true, "", (image.Bytes, imageName, image.ContentType));
            }
            catch (Exception ex)
            {
                return new ResponseModelFactory().CreateResponseModel<(byte[] FileBytes, string FileName, string ContentType)>(false, ex.Message, default);
            }
        }

        public ResponseModel<StudentDashboardDto> GetDashboard(string studentId)
        {
            var studentCourseIds = context.CourseEnrollments
                .AsNoTracking()
                .Where(e => e.StuId == studentId)
                .Select(e => e.CrsId)
                .ToList();

            if (!studentCourseIds.Any())
            {
                var emptyDashboard = new StudentDashboardDto
                {
                    MyCourses = 0,
                    PendingAssignments = 0,
                    DueTomorrow = 0,
                    AverageGrade = 0,
                    UpcomingDeadlines = new List<UpcomingAssignmentDto>(),
                    RecentGrades = new List<RecentGradeDto>()
                };
                return new ResponseModelFactory()
                    .CreateResponseModel<StudentDashboardDto>(true, "", emptyDashboard);
            }

            int myCourses = studentCourseIds.Count;

            var studentSubmissionAssignmentIds = context.Submissions
                .AsNoTracking()
                .Where(s => s.StuId == studentId)
                .Select(s => s.AssignmentId)
                .Distinct()
                .ToList();

            int pendingAssignments = context.Assignments
                .AsNoTracking()
                .Where(a => studentCourseIds.Contains(a.CrsId)
                            && a.Status == AssignmentStatus.Published
                            && !studentSubmissionAssignmentIds.Contains(a.Id))
                .Count();

            int dueTomorrow = context.Assignments
                .AsNoTracking()
                .Where(a => studentCourseIds.Contains(a.CrsId)
                            && a.Status == AssignmentStatus.Published
                            && a.DeadLine > DateTime.Now
                            && a.DeadLine <= DateTime.Now.AddDays(1))
                .Count();

            var studentGraded = context.Submissions
                .AsNoTracking()
                .Where(s => s.StuId == studentId && s.grade != null);

            double averageGrade = studentGraded.Any() ? Math.Round(studentGraded.Average(s => s.grade.Value), 1) : 0.0;

            var upcomingAssignments = context.Assignments
                .AsNoTracking()
                .Include(a => a.course)
                .Where(a => studentCourseIds.Contains(a.CrsId) && a.Status == AssignmentStatus.Published && a.DeadLine >= DateTime.Now)
                .OrderBy(a => a.DeadLine)
                .Take(10)
                .ToList();

            var upcomingDtos = upcomingAssignments.Select(a =>
            {
                var sub = context.Submissions.AsNoTracking().FirstOrDefault(s => s.AssignmentId == a.Id && s.StuId == studentId);
                string statusStr = sub != null ? (sub.Status == SubmissionStatus.Graded ? "Graded" : "Submitted") : "Pending";
                TimeSpan remaining = a.DeadLine - DateTime.Now;
                string remainingStr = remaining.TotalHours < 1 ? $"{Math.Max(0, remaining.Minutes)} mins" :
                                      remaining.TotalHours < 24 ? $"{remaining.Hours} hours" :
                                      $"{remaining.Days} days {remaining.Hours} hours";

                return new UpcomingAssignmentDto
                {
                    AssignmentId = a.Id,
                    Title = a.Title,
                    CourseId = a.CrsId,
                    CourseName = a.course != null ? a.course.CrsName : "",
                    Deadline = a.DeadLine,
                    RemainingTime = remainingStr,
                    Status = statusStr
                };
            }).ToList();

            var recentGrades = context.Submissions
                .AsNoTracking()
                .Include(s => s.assignment)
                .ThenInclude(a => a.course)
                .Where(s => s.StuId == studentId && s.grade != null)
                .OrderByDescending(s => s.SubmitedAt)
                .Take(5)
                .Select(s => new RecentGradeDto
                {
                    AssignmentId = s.AssignmentId,
                    AssignmentTitle = s.assignment != null ? s.assignment.Title : "",
                    CourseId = s.assignment != null ? s.assignment.CrsId : 0,
                    CourseName = (s.assignment != null && s.assignment.course != null) ? s.assignment.course.CrsName : "",
                    Grade = s.grade.Value,
                    GradedAt = s.SubmitedAt
                })
                .ToList();

            var dashboardDto = new StudentDashboardDto
            {
                MyCourses = myCourses,
                PendingAssignments = pendingAssignments,
                DueTomorrow = dueTomorrow,
                AverageGrade = averageGrade,
                UpcomingDeadlines = upcomingDtos,
                RecentGrades = recentGrades
            };

            return new ResponseModelFactory()
                .CreateResponseModel<StudentDashboardDto>(true, "", dashboardDto);
        }
    }
}
