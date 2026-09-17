using Assignment__Management_System.DataLayer;
using Assignment__Management_System.DataLayer.DTOs;
using Assignment__Management_System.Factories;
using Assignment__Management_System.Models;
using Assignment__Management_System.Models.Data;
using Assignment__Management_System.Models.Entities;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace Assignment__Management_System.Services
{
    public class InstructorService : IInstructorService
    {
        private const long MaxFileSize = 10 * 1024 * 1024;
        private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".pdf", ".doc", ".docx", ".txt", ".zip", ".rar",
            ".cs", ".cpp", ".c", ".java", ".py", ".js", ".ts",
            ".html", ".css", ".json", ".xml", ".sql",
            ".png", ".jpg", ".jpeg"
        };

        private readonly AppDbContext _context;
        private readonly TokenRequestModel Request;
        private readonly INotificationService _notificationService;
        private readonly IWebHostEnvironment _environment;

        public InstructorService(
            AppDbContext context,
            INotificationService notificationService,
            IWebHostEnvironment environment)
        {
            _context = context;
            _notificationService = notificationService;
            _environment = environment;
        }

        public ResponseModel<AssignmentDTO> AddAssignmentToCourse(string userid, AssignmentDTO model)
        {
            if(model.DeadLine < DateOnly.FromDateTime(DateTime.Now))
                return new ResponseModelFactory()
                    .CreateResponseModel<AssignmentDTO>(false, "Deadline cannot be in the past!", null);

            if (!_context.Courses.Any(c => c.CrsId == model.CrsId))
                return new ResponseModelFactory()
                    .CreateResponseModel<AssignmentDTO>(false, "Course Not Found!", null);

            if (model.File == null || model.File.Length == 0)
                return new ResponseModelFactory()
                    .CreateResponseModel<AssignmentDTO>(false, "Assignment file is required!", null);

            if (model.File.Length > MaxFileSize)
                return new ResponseModelFactory()
                    .CreateResponseModel<AssignmentDTO>(false, "Assignment file cannot exceed 10 MB!", null);

            var originalFileName = Path.GetFileName(model.File.FileName);
            var extension = Path.GetExtension(originalFileName);

            if (string.IsNullOrWhiteSpace(originalFileName) || !AllowedExtensions.Contains(extension))
                return new ResponseModelFactory()
                    .CreateResponseModel<AssignmentDTO>(false, "Unsupported assignment file type!", null);

            var storedFileName = $"{Guid.NewGuid():N}_{originalFileName}";
            var uploadDirectory = Path.Combine(_environment.ContentRootPath, "App_Data", "Assignments");
            var storedFilePath = Path.Combine(uploadDirectory, storedFileName);

            try
            {
                Directory.CreateDirectory(uploadDirectory);

                using (var stream = new FileStream(storedFilePath, FileMode.CreateNew))
                {
                    model.File.CopyTo(stream);
                }

                var assignment = new Assignment()
                {
                    Title = model.Title,
                    DeadLine = model.DeadLine,
                    CrsId = Convert.ToInt32(model.CrsId),
                    FilePath = storedFileName
                };

                _context.Assignments.Add(assignment);
                _context.SaveChanges();

                _notificationService.NotifyStudentsOfNewAssignment(assignment);

                model.AssignmentId = assignment.Id;
                model.CrsName = _context.Courses
                    .Where(c => c.CrsId == model.CrsId)
                    .Select(c => c.CrsName)
                    .FirstOrDefault();
                model.FileName = originalFileName;
                model.File = null;

                return new ResponseModelFactory()
                    .CreateResponseModel<AssignmentDTO>(true,"Adding Successfully",model);
            }
            catch (Exception ex)
            {
                if (System.IO.File.Exists(storedFilePath))
                    System.IO.File.Delete(storedFilePath);

                return new ResponseModelFactory()
                    .CreateResponseModel<AssignmentDTO>(false, ex.Message, null);
            }
        }

        public ResponseModel<(byte[] FileBytes, string FileName, string ContentType)> GetAssignmentFile(int assignmentId)
        {
            var assignment = _context.Assignments
                .AsNoTracking()
                .FirstOrDefault(a => a.Id == assignmentId);

            if (assignment == null)
                return new ResponseModelFactory()
                    .CreateResponseModel<(byte[] FileBytes, string FileName, string ContentType)>(false, "Assignment Not Found!", default);

            if (string.IsNullOrWhiteSpace(assignment.FilePath))
                return new ResponseModelFactory()
                    .CreateResponseModel<(byte[] FileBytes, string FileName, string ContentType)>(false, "No file attached to this assignment!", default);

            var storedFileName = Path.GetFileName(assignment.FilePath);
            var filePath = Path.Combine(_environment.ContentRootPath, "App_Data", "Assignments", storedFileName);

            if (!System.IO.File.Exists(filePath))
                return new ResponseModelFactory()
                    .CreateResponseModel<(byte[] FileBytes, string FileName, string ContentType)>(false, "Assignment file not found on server!", default);

            try
            {
                var bytes = System.IO.File.ReadAllBytes(filePath);
                var originalFileName = GetOriginalFileName(storedFileName);
                var contentType = GetContentType(Path.GetExtension(originalFileName));

                return new ResponseModelFactory()
                    .CreateResponseModel<(byte[] FileBytes, string FileName, string ContentType)>(
                        true, "", (bytes, originalFileName, contentType));
            }
            catch (Exception ex)
            {
                return new ResponseModelFactory()
                    .CreateResponseModel<(byte[] FileBytes, string FileName, string ContentType)>(false, ex.Message, default);
            }
        }

        private static string GetOriginalFileName(string storedFileName)
        {
            var separatorIndex = storedFileName.IndexOf('_');
            return separatorIndex >= 0
                ? storedFileName[(separatorIndex + 1)..]
                : storedFileName;
        }

        private static string GetContentType(string extension)
        {
            return extension.ToLowerInvariant() switch
            {
                ".pdf" => "application/pdf",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".txt" => "text/plain",
                ".zip" => "application/zip",
                ".rar" => "application/vnd.rar",
                ".cs" or ".cpp" or ".c" or ".java" or ".py" or ".js" or ".ts" or ".html" or ".css" or ".json" or ".xml" or ".sql" => "text/plain",
                ".png" => "image/png",
                ".jpg" or ".jpeg" => "image/jpeg",
                _ => "application/octet-stream"
            };
        }

        public ResponseModel<AssignmentDTO> UpdateAssignmentsGrades(int submissionId,double Grade)
        {
            if(Grade < 0 || Grade > 10)
                return new ResponseModelFactory()
                    .CreateResponseModel<AssignmentDTO>(false, "Grade must be between 0 and 10!", null);

            if(!_context.Submissions.Any(s => s.SubId == submissionId))
                return new ResponseModelFactory()
                    .CreateResponseModel<AssignmentDTO>(false, "Submission Not Found!", null);

            try
            {
                var sub = _context.Submissions.AsNoTracking()
                .FirstOrDefault(s => s.SubId == submissionId);

                sub.grade = Grade;
                _context.Update(sub);
                _context.SaveChanges();

                return new ResponseModelFactory()
                     .CreateResponseModel<AssignmentDTO>(true,"Updat Grades success", null);
            }
            catch(Exception ex)
            {
                return new ResponseModelFactory()
                     .CreateResponseModel<AssignmentDTO>(false, ex.Message, null);
            }
        }

        public ResponseModel<IQueryable<Submission>> GetSubmissions(int AssignId)
        {
            var subs = _context.Submissions.AsNoTracking()
                .Include(s => s.assignment)
                .Include(s => s.student)
                .ThenInclude(s => s.User)
                .Where(s => s.AssignmentId == AssignId);

            if (subs != null)
                return new ResponseModelFactory()
                    .CreateResponseModel<IQueryable<Submission>>(true, "", subs);
            else
                return new ResponseModelFactory()
                    .CreateResponseModel<IQueryable<Submission>>(false, "No available submissions!", null);
        }

        public ResponseModel<IQueryable<AssignmentStudentGrades>> GetAssignmentStudentGrades(int assignmentid)
        {
            var studgrades = _context.Submissions
                .AsNoTracking()
                .Include(s => s.student)
                .ThenInclude(s => s.User)
                .Where(s => s.AssignmentId == assignmentid)
                .Select(s => new AssignmentStudentGrades()
                {
                    StudentName = s.student.User.Name,
                    Grade = s.grade,
                });

            if (studgrades != null)
                return new ResponseModelFactory()
                  .CreateResponseModel<IQueryable<AssignmentStudentGrades>>(true, "", studgrades);
            else
                return new ResponseModelFactory()
                  .CreateResponseModel<IQueryable<AssignmentStudentGrades>>(false, "No Submission available for this assignment!", null);
        }

        public ResponseModel<IQueryable<InstructorDTO>> GetInstructors()
        {
            var insts = _context.Instructors
                .AsNoTracking()
                .Include(i => i.User)
                .Select(i => new InstructorDTO()
                {
                    Name = i.User.Name,
                    id = i.Id
                });

            if (insts != null)
                return new ResponseModelFactory()
                    .CreateResponseModel<IQueryable<InstructorDTO>>(true, "", insts);
            else
                return new ResponseModelFactory()
                    .CreateResponseModel<IQueryable<InstructorDTO>>(false, "No Instructors available!", null);
        }

        public ResponseModel<IQueryable<CourseDto>> GetInstructorCourses(string instid)
        {
            var course = _context.Courses
                            .AsNoTracking()
                            .Include(c => c.instructor)
                            .ThenInclude(c => c.User)
                            .Where(c => c.InstId == instid)
                            .Select(x => new CourseDto()
                            {
                                Id = x.CrsId,
                                CrsName = x.CrsName,
                                InstId = x.InstId,
                                InstName = x.instructor.User.Name
                            });

            if (course == null)
                return new ResponseModelFactory()
                 .CreateResponseModel<IQueryable<CourseDto>>(false, "No Available Courses!", null);
            else
                return new ResponseModelFactory()
                 .CreateResponseModel<IQueryable<CourseDto>>(true, "", course);
        }
    }
}
