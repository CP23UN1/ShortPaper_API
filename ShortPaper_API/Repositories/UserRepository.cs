using ShortPaper_API.Entities;

namespace ShortPaper_API.Repositories
{
    public class UserRepository
    {
        public int UserId { get; set; }
        public string? StudentId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Year { get; set; }
        //public SubjectRepository RegisteredSubjectid { get; set; }
        //public SubjectRepository ShortpaperSubjectid { get; set; }
        
        //public int RegisteredSubjectid { get; set; }
        //public int ShortpaperSubjectid { get; set; }

        public Subject RegisteredSubject { get; set; }
        public Subject ShortpaperSubject { get; set; }

    }
}
