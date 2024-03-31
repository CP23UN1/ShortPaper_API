using Microsoft.AspNetCore.Mvc;
using ShortPaper_API.DTO;
using ShortPaper_API.Helper;

namespace ShortPaper_API.Services.Committees
{
    public interface ICommitteeService
    {
        Task<ServiceResponse<List<CommitteeDTO>>> GetCommitteesAsync();
        Task<ServiceResponse<CommitteeDTO>> GetCommitteesById(string committeeId);
        Task<ServiceResponse<List<AddCommitteeDTO>>> AddCommitteesFromCsvAsync(IFormFile csvFile);
        Task<ServiceResponse<List<AddCommitteeForStudentDTO>>> AddCommitteesForStudentsFromCsvAsync(IFormFile csvFile);
        Task<ServiceResponse<List<CommitteeRoleDTO>>> UpdateCommitteeRolesForStudentAsync(string studentId, List<CommitteeRoleDTO> committeeRoles);
    }
}
