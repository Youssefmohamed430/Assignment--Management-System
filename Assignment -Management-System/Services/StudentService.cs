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
                .Select(s => new UserDto { UserName = s.UserName, Name = s.Name, Email = s.Email }).FirstOrDefault();

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
    }
}
