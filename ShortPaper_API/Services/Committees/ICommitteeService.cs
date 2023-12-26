using ShortPaper_API.DTO;
using ShortPaper_API.Helper;

namespace ShortPaper_API.Services.Committees
{
    public interface ICommitteeService
    {
        Task<ServiceResponse<List<CommitteeDTO>>> GetCommitteesAsync();
        Task<ServiceResponse<List<AddCommitteeDTO>>> AddCommitteesFromCsvAsync(IFormFile csvFile);
    }
}
