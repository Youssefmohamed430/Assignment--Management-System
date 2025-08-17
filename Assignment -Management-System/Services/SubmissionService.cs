using Assignment__Management_System.DataLayer.DTOs;
using Assignment__Management_System.Models.Data;
using Assignment__Management_System.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Immutable;

namespace Assignment__Management_System.Services
{
    public class SubmissionService : ISubmissionService
    {
        private readonly AppDbContext context;

        public SubmissionService(AppDbContext context)
        {
            this.context = context;
        }

        public string SubmitAssignment(SubmitDTO Sub)
        {
            try
            {
                var submit = new Submission()
                {
                    FilePath = @"E:\Submissions\" + Sub.FileName,
                    grade = null,
                    StuId = Sub.StudId,
                    AssignmentId = Sub.AssignmentId,
                };

                context.Submissions.Add(submit);

                context.SaveChanges();

                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public IQueryable<SubmitDTO> GetSubs(int assignid)
        {
            var subs = context.Submissions.AsNoTracking()
                .Include(s => s.student)
                .Include(s => s.assignment)
                .Where(s => s.AssignmentId == assignid)
                .Select(x => new SubmitDTO {
                    StudId = x.StuId,
                    AssignmentId = x.AssignmentId,
                    FileName = Path.GetFileName(x.FilePath),
                });

            return subs;
        }
    }
}
