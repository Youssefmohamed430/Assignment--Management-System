using Assignment__Management_System.DataLayer;
using Assignment__Management_System.DataLayer.DTOs;
using Assignment__Management_System.Models.Entities;

namespace Assignment__Management_System.Services
{
    public interface IAssignmentService
    {
        ResponseModel<IQueryable<AssignmentDTO>> GetAssignments(int CrsId);
        ResponseModel<AssignmentDTO> GetAssignmentById(int assignmentid);
        ResponseModel<AssignmentDTO> UpdateAssignment(AssignmentDTO assignment,int id);
        ResponseModel<AssignmentDTO> DeleteAssignment(int assignmentid);
    } 
}
