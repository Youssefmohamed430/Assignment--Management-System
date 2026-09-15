using Assignment__Management_System.DataLayer;
using Assignment__Management_System.DataLayer.DTOs;
using Assignment__Management_System.Factories;
using Assignment__Management_System.Models.Data;
using Assignment__Management_System.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Assignment__Management_System.Services
{
    public class SubmissionService : ISubmissionService
    {
        private const long MaxFileSize = 10 * 1024 * 1024; // 10 MB
        private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".pdf", ".doc", ".docx", ".txt", ".zip", ".rar",
            ".cs", ".cpp", ".c", ".java", ".py", ".js", ".ts",
            ".html", ".css", ".json", ".xml", ".sql",
            ".png", ".jpg", ".jpeg"
        };

        private readonly AppDbContext context;
        private readonly IWebHostEnvironment environment;

        public SubmissionService(AppDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }

        public ResponseModel<SubmitDTO> SubmitAssignment(SubmitDTO sub, string stuid)
        {
            try
            {
                if (context.Submissions.Any(x => x.StuId == stuid && x.AssignmentId == sub.AssignmentId))
                    return new ResponseModelFactory()
                        .CreateResponseModel<SubmitDTO>(false, "You have already submitted this assignment!", null);

                var dateassignment = context.Assignments
                    .Where(a => a.Id == sub.AssignmentId)
                    .Select(a => a.DeadLine)
                    .FirstOrDefault();

                if (dateassignment < DateOnly.FromDateTime(DateTime.Now))
                    return new ResponseModelFactory()
                        .CreateResponseModel<SubmitDTO>(false, "You cannot submit this assignment, deadline has passed!", null);

                if (sub.File is null || sub.File.Length == 0)
                    return new ResponseModelFactory()
                        .CreateResponseModel<SubmitDTO>(false, "Please select a file to submit!", null);

                if (sub.File.Length > MaxFileSize)
                    return new ResponseModelFactory()
                        .CreateResponseModel<SubmitDTO>(false, "File size cannot exceed 10 MB!", null);

                var extension = Path.GetExtension(sub.File.FileName);
                if (string.IsNullOrWhiteSpace(extension) || !AllowedExtensions.Contains(extension))
                    return new ResponseModelFactory()
                        .CreateResponseModel<SubmitDTO>(false, "This file type is not allowed!", null);

                var originalFileName = Path.GetFileName(sub.File.FileName);
                var storedFileName = $"{Guid.NewGuid():N}{extension}";

                var submissionsDirectory = Path.Combine(
                    environment.ContentRootPath,
                    "App_Data",
                    "Submissions");

                Directory.CreateDirectory(submissionsDirectory);

                var filePath = Path.Combine(submissionsDirectory, storedFileName);

                using (var stream = new FileStream(filePath, FileMode.CreateNew))
                {
                    sub.File.CopyTo(stream);
                }

                var submit = new Submission
                {
                    // Only the stored file name is saved in the database.
                    FilePath = storedFileName,
                    grade = null,
                    StuId = stuid,
                    AssignmentId = sub.AssignmentId,
                };

                context.Submissions.Add(submit);
                context.SaveChanges();

                return new ResponseModelFactory()
                    .CreateResponseModel<SubmitDTO>(true, "Submitted Successfully!", new SubmitDTO
                    {
                        AssignmentId = submit.AssignmentId,
                        FileName = originalFileName,
                        SubmissionId = submit.SubId
                    });
            }
            catch (Exception)
            {
                return new ResponseModelFactory()
                    .CreateResponseModel<SubmitDTO>(false, "Submitted Failed!", null);
            }
        }

        public ResponseModel<IQueryable<SubmitDTO>> GetSubs(int assignid)
        {
            var subs = context.Submissions.AsNoTracking()
                .Include(s => s.student)
                .ThenInclude(s => s.User)
                .Include(s => s.assignment)
                .Where(s => s.AssignmentId == assignid)
                .Select(x => new SubmitDTO
                {
                    SubmissionId = x.SubId,
                    stuname = x.student.User.Name,
                    AssignmentId = x.AssignmentId,
                    AssignmentTitle = x.assignment.Title,
                    FileName = Path.GetFileName(x.FilePath),
                    grade = x.grade
                });

            if (subs.Any())
                return new ResponseModelFactory()
                    .CreateResponseModel<IQueryable<SubmitDTO>>(true, "", subs);
            else
                return new ResponseModelFactory()
                    .CreateResponseModel<IQueryable<SubmitDTO>>(false, "No Submits!", null);
        }

        public ResponseModel<(byte[] FileBytes, string FileName, string ContentType)> GetSubmissionFile(int submissionId)
        {
            var submission = context.Submissions
                .AsNoTracking()
                .FirstOrDefault(s => s.SubId == submissionId);

            if (submission is null)
                return new ResponseModelFactory()
                    .CreateResponseModel<(byte[] FileBytes, string FileName, string ContentType)>(false, "Submission not found!", default);

            var storedFileName = Path.GetFileName(submission.FilePath);
            if (string.IsNullOrWhiteSpace(storedFileName) || !string.Equals(storedFileName, submission.FilePath, StringComparison.Ordinal))
                return new ResponseModelFactory()
                    .CreateResponseModel<(byte[] FileBytes, string FileName, string ContentType)>(false, "Invalid file path!", default);

            var submissionsDirectory = Path.Combine(environment.ContentRootPath, "App_Data", "Submissions");
            var filePath = Path.Combine(submissionsDirectory, storedFileName);

            if (!System.IO.File.Exists(filePath))
                return new ResponseModelFactory()
                    .CreateResponseModel<(byte[] FileBytes, string FileName, string ContentType)>(false, "Submission file not found!", default);

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var extension = Path.GetExtension(storedFileName);
            var contentType = GetContentType(extension);

            return new ResponseModelFactory()
                .CreateResponseModel<(byte[] FileBytes, string FileName, string ContentType)>(
                    true,
                    "",
                    (fileBytes, storedFileName, contentType));
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
                ".png" => "image/png",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".json" => "application/json",
                ".xml" => "application/xml",
                _ => "application/octet-stream"
            };
        }
    }
}
