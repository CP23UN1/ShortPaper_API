
using ShortPaper_API.Entities;

namespace ShortPaper_API.Services.Subjects
{
    public class SubjectService : ISubjectService
    {
        private readonly ShortpaperDbContext _db;
        public SubjectService(ShortpaperDbContext db) 
        {
            _db = db;
        }

        public List<Subject> GetSubjects()
        {
            var subjects = (from a in _db.Subjects
                            select a).ToList();

            return subjects;
        }

        public Subject GetSubject(int id)
        {
            var subject = (from a in _db.Subjects
                           where a.Id == id
                           select a).FirstOrDefault();

            return subject;
        }

    }
}
