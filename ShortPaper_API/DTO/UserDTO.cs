using ShortPaper_API.Entities;

namespace ShortPaper_API.DTO
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string? StudentId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string? Role { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Year { get; set; }
        public int? RegisteredSubjectid { get; set; }
        public int? ShortpaperSubjectid { get; set; }
        public Subject? RegisteredSubject { get; set; }
        public Subject? ShortpaperSubject { get; set; }
        public string ProjectName { get; set; }
    }
}
