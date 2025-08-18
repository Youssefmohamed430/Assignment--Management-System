using Assignment__Management_System.DataLayer;
using Assignment__Management_System.DataLayer.DTOs;
using Assignment__Management_System.Factories;
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

        public ResponseModel<SubmitDTO> SubmitAssignment(SubmitDTO Sub)
        {
            try
            {
                if (context.Submissions.Any(x => x.StuId == Sub.StudId && x.AssignmentId == Sub.AssignmentId))
                    return new ResponseModelFactory()
                        .CreateResponseModel<SubmitDTO>(false, "You have already submitted this assignment!", null);

                var submit = new Submission()
                {
                    FilePath = @"E:\Submissions\" + Sub.FileName,
                    grade = null,
                    StuId = Sub.StudId,
                    AssignmentId = Sub.AssignmentId,
                };

                context.Submissions.Add(submit);

                context.SaveChanges();

                return new ResponseModelFactory()
                    .CreateResponseModel<SubmitDTO>(true,"Submitted Successfully!",Sub);
            }
            catch (Exception ex)
            {
                return new ResponseModelFactory()
                    .CreateResponseModel<SubmitDTO>(false, "Submitted Failed!", null);
            }
        }
        public ResponseModel<IQueryable<SubmitDTO>> GetSubs(int assignid)
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

            if (subs.Any())
                return new ResponseModelFactory()
                    .CreateResponseModel<IQueryable<SubmitDTO>>(true, "", subs);
            else
                return new ResponseModelFactory()
                    .CreateResponseModel<IQueryable<SubmitDTO>>(false, "No Submits!", null);
        }
    }
}
