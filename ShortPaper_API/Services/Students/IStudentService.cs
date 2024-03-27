using ShortPaper_API.DTO;
using ShortPaper_API.Entities;
using ShortPaper_API.Helper;

namespace ShortPaper_API.Services.Students
{
    public interface IStudentService
    {
        ServiceResponse<List<StudentDTO>> GetStudents();
        ServiceResponse<List<StudentDTO>> GetStudentByFilter(string searchText);
        ServiceResponse<StudentDTO> GetStudent(string id);
        ServiceResponse<List<StudentDTO>> GetStudentByCommittee(string committeeId);
        ServiceResponse<List<StudentDTO>> GetStudentByCommitteeAndFilter(string committeeId, string filterText);
        ServiceResponse<CreateStudentDTO> CreateStudent(CreateStudentDTO studentDTO);
        Task<ServiceResponse<List<CreateStudentDTO>>> AddStudentsFromCsvAsync(IFormFile csvFile);
        ServiceResponse<UpdateStudentDTO> UpdateStudent(UpdateStudentDTO studentDTO);
        Student DeleteStudent(string id);

        ServiceResponse<List<string>> GetUniqueYears();

    }
}
