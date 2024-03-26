using ShortPaper_API.DTO;
using ShortPaper_API.Helper;

namespace ShortPaper_API.Services.Subjects
{
    public interface ISubjectService
    {
        ServiceResponse<List<SubjectDTO>> GetSubjects();
        ServiceResponse<List<SubjectDTO>> GetSubjectByFilter(string text);
        ServiceResponse<UpdateSubjectDTO> UpdateStudentSubject(string studentId, UpdateSubjectDTO subject);
    }
}
