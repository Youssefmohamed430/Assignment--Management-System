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

        public CourseService(AppDbContext context)
        {
            _context = context;
        }
        public ResponseModel<IQueryable<CourseDto>> GetCourses()
        {
            var course = _context.Courses
                            .Select( x => new CourseDto()
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

        public ResponseModel<CourseDto> AddCourses(CourseDto model)
        {
            var result = _context.Courses.Any(c => c.CrsName == model.CrsName);

            if (!result)
            {
                Course course = new Course()
                {
                    CrsName = model.CrsName,
                    InstId = model.InstId,
                };

                try
                {
                    _context.Courses.Add(course);

                    _context.SaveChanges();

                    return new ResponseModelFactory()
                        .CreateResponseModel<CourseDto>(true, "Adding Successfully", model);
                }
                catch (Exception ex)
                {
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
            if (result)
            {
                var course = new CourseEnrollments() { CrsId = model.CrsId, StuId = userid};
                try
                {
                    _context.CourseEnrollments.Add(course);

                    _context.SaveChanges();

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
                var oldCrs = _context.Courses.FirstOrDefault(a => a.CrsId == crsid);

                oldCrs.CrsName = model.CrsName;
                oldCrs.InstId = model.InstId;

                _context.Courses.Update(oldCrs);

                _context.SaveChanges();

                return new ResponseModelFactory()
                    .CreateResponseModel<CourseDto>(true, "Updated Successfully!", model);
            }
            catch (Exception ex)
            {
                return new ResponseModelFactory()
                    .CreateResponseModel<CourseDto>(false, ex.Message, null);
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
