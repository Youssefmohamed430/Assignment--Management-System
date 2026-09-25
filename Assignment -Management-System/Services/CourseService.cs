using Assignment__Management_System.DataLayer;
using Assignment__Management_System.DataLayer.DTOs;
using Assignment__Management_System.Factories;
using Assignment__Management_System.Models.Data;
using Assignment__Management_System.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace Assignment__Management_System.Services
{
    public class CourseService : ICourseService
    {
        private readonly AppDbContext _context;
        private readonly ImageStorageService _imageStorage;

        public CourseService(AppDbContext context, ImageStorageService imageStorage)
        {
            _context = context;
            _imageStorage = imageStorage;
        }

        public ResponseModel<CourseDto> GetCourseById(int id)
        {
            var course = _context.Courses
                .Where(c => c.CrsId == id)
                .Select(x => new CourseDto()
                {
                    Id = x.CrsId,
                    CrsName = x.CrsName,
                    InstId = x.InstId,
                    InstName = x.instructor.User.Name,
                    ImageName = x.ImagePath,
                    ImageUrl = $"/api/Course/Image/{x.CrsId}"
                })
                .FirstOrDefault();

            if (course == null)
                return new ResponseModelFactory()
                 .CreateResponseModel<CourseDto>(false, "Course Not Found!", null);
            else
                return new ResponseModelFactory()
                 .CreateResponseModel<CourseDto>(true, "", course);
        }
        public ResponseModel<IQueryable<CourseDto>> GetCourses()
        {
            var course = _context.Courses
                .Select(x => new CourseDto()
                {
                    Id = x.CrsId,
                    CrsName = x.CrsName,
                    InstId = x.InstId,
                    InstName = x.instructor.User.Name,
                    ImageName = x.ImagePath,
                    ImageUrl = $"/api/Course/Image/{x.CrsId}"
                });

            if (course == null)
                return new ResponseModelFactory()
                 .CreateResponseModel<IQueryable<CourseDto>>(false, "No Available Courses!", null);
            else
                return new ResponseModelFactory()
                 .CreateResponseModel<IQueryable<CourseDto>>(true, "", course);
        }

        public ResponseModel<CourseDto> AddCourses(CourseDto model)
        {
            if (model.Image == null || model.Image.Length == 0)
                return new ResponseModelFactory().CreateResponseModel<CourseDto>(false, "Course image is required!", null);
            var result = _context.Courses.Any(c => c.CrsName == model.CrsName);

            if (!result)
            {
                Course course = new Course()
                {
                    CrsName = model.CrsName,
                    InstId = model.InstId,
                };

                string storedImage;
                try
                {
                    storedImage = _imageStorage.SaveImage(model.Image, "Courses");
                    course.ImagePath = storedImage;
                }
                catch (Exception ex)
                {
                    return new ResponseModelFactory().CreateResponseModel<CourseDto>(false, ex.Message, null);
                }

                try
                {
                    _context.Courses.Add(course);

                    _context.SaveChanges();

                    model.Id = course.CrsId;
                    model.ImageName = course.ImagePath;
                    model.Image = null;

                    return new ResponseModelFactory()
                        .CreateResponseModel<CourseDto>(true, "Adding Successfully", model);
                }
                catch (Exception ex)
                {
                    _imageStorage.DeleteImage("Courses", course.ImagePath);
                    return new ResponseModelFactory()
                        .CreateResponseModel<CourseDto>(false,ex.Message, null);
                }
            }
            return new ResponseModelFactory()
                        .CreateResponseModel<CourseDto>(false, "This Course already exist!", null);
        }

        public ResponseModel<CourseEnrollDTO> EnrollCourse(CourseEnrollDTO model, string userid)
        {
            var result = _context.CourseEnrollments.Any(c => c.StuId == userid && c.CrsId == model.CrsId);
            if (!result)
            {
                var course = new CourseEnrollments() { CrsId = model.CrsId, StuId = userid};
                try
                {
                    _context.CourseEnrollments.Add(course);

                    _context.SaveChanges();

                    model.CrsName = _context.CourseEnrollments?
                        .Where(c => c.CrsId == model.CrsId)
                        .Include(c => c.course)
                        .FirstOrDefault()?
                        .course?.CrsName;   

                    return new ResponseModelFactory()
                         .CreateResponseModel<CourseEnrollDTO>(true,"", model);
                }
                catch (Exception ex)
                {
                    return new ResponseModelFactory()
                         .CreateResponseModel<CourseEnrollDTO>(false, ex.Message, null);
                }
            }
            else
                return new ResponseModelFactory()
                         .CreateResponseModel<CourseEnrollDTO>(false, "Course is already enrolled!", null);
        }

        public ResponseModel<CourseDto> UpdateCourses(CourseDto model, int crsid)
        {
            try
            {
                var oldCrs = _context.Courses
                    .FirstOrDefault(a => a.CrsId == crsid);

                oldCrs.CrsName = model.CrsName;
                oldCrs.InstId = model.InstId;

                _context.Courses.Update(oldCrs);

                _context.SaveChanges();

                var coursedto = _context.Courses
                    .Include(c => c.instructor)
                    .ThenInclude(c => c.User)
                    .Where(c => c.CrsId == crsid)
                    .Select(c => new CourseDto()
                    {
                        Id = c.CrsId,
                        CrsName = c.CrsName,
                        InstId = c.InstId,
                        InstName = c.instructor.User.Name,
                        ImageName = c.ImagePath
                    }).FirstOrDefault();

                return new ResponseModelFactory()
                    .CreateResponseModel<CourseDto>(true, "Updated Successfully!", coursedto);
            }
            catch (Exception ex)
            {
                return new ResponseModelFactory()
                    .CreateResponseModel<CourseDto>(false, ex.Message, null);
            }
        }

        public ResponseModel<(byte[] FileBytes, string FileName, string ContentType)> GetCourseImage(int courseId)
        {
            var imageName = _context.Courses.AsNoTracking()
                .Where(c => c.CrsId == courseId)
                .Select(c => c.ImagePath)
                .FirstOrDefault();

            if (string.IsNullOrWhiteSpace(imageName))
                return new ResponseModelFactory().CreateResponseModel<(byte[] FileBytes, string FileName, string ContentType)>(
                    false, "No course image found!", default);

            try
            {
                var image = _imageStorage.ReadImage("Courses", imageName);
                return new ResponseModelFactory().CreateResponseModel<(byte[] FileBytes, string FileName, string ContentType)>(
                    true, "", (image.Bytes, imageName, image.ContentType));
            }
            catch (Exception ex)
            {
                return new ResponseModelFactory().CreateResponseModel<(byte[] FileBytes, string FileName, string ContentType)>(
                    false, ex.Message, default);
            }
        }

        public ResponseModel<CourseDto> DeleteCourses(int crsid)
        {
            try
            {
                var Crs = _context.Courses.FirstOrDefault(a => a.CrsId == crsid);

                _context.Courses.Remove(Crs);

                _context.SaveChanges();

                return new ResponseModelFactory()
                    .CreateResponseModel<CourseDto>(true, "Deleted Successfully!", null);
            }
            catch (Exception ex)
            {
                return new ResponseModelFactory()
                    .CreateResponseModel<CourseDto>(false, ex.Message, null);
            }
        }
    }
}
