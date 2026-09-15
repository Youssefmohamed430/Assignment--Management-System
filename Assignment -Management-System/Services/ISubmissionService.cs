using Assignment__Management_System.DataLayer;
using Assignment__Management_System.DataLayer.DTOs;
using Assignment__Management_System.Models.Entities;

namespace Assignment__Management_System.Services
{
    public interface ISubmissionService
    {
        ResponseModel<SubmitDTO> SubmitAssignment(SubmitDTO Sub, string stuid);
        ResponseModel<IQueryable<SubmitDTO>> GetSubs(int assignid);
        ResponseModel<(byte[] FileBytes, string FileName, string ContentType)> GetSubmissionFile(int submissionId);
    }
}
