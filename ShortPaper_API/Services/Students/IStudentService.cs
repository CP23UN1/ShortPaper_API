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
        ServiceResponse<CreateStudentDTO> CreateStudent(CreateStudentDTO studentDTO);
        ServiceResponse<UpdateStudentDTO> UpdateStudent(UpdateStudentDTO studentDTO);
        Student DeleteStudent(string id);

    }
}
