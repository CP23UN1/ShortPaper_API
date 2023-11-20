using ShortPaper_API.Entities;

namespace ShortPaper_API.Services.Subjects
{
    public interface ISubjectService
    {
        List<Subject> GetSubjects();
        Subject GetSubject(int id);
    }
}
