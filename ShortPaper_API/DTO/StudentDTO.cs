using ShortPaper_API.Entities;

namespace ShortPaper_API.DTO
{
    public class StudentDTO
    {
        public string StudentId { get; set; } = null!;

        public string Firstname { get; set; } = null!;

        public string Lastname { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? Password { get; set; }

        public string? AlternativeEmail { get; set; }

        public string? Phonenumber { get; set; }

        public string Year { get; set; } = null!;

        public ShortpaperForStudentDTO? Shortpaper { get; set; }

        public List<SubjectDTO> Subjects { get; set; } = new List<SubjectDTO>();

        public List<CommitteeDTO> Committees { get; set; } = new List<CommitteeDTO>();

        public List<ShortpaperFileDTO> ShortpaperFiles { get; set; } = new List<ShortpaperFileDTO>();

        public SubjectDTO? Subject { get; set; }

        public List<ShortpaperFileDTO>? ShortpaperFile { get; set; }

        public List<CommitteeDTO>? Committee { get; set; }

        //public virtual ICollection<Shortpaper> Shortpapers { get; set; } = new List<Shortpaper>();
    }
}
