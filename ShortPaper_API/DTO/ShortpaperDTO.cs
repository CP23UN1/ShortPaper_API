using ShortPaper_API.Entities;

namespace ShortPaper_API.DTO
{
    public class ShortpaperDTO
    {
        public int ShortpaperId { get; set; }

        public string? ShortpaperTopic { get; set; }

        public List<SubjectDTO> Subjects { get; set; } = new List<SubjectDTO>();

        public List<StudentForShortpaperDTO>? Student { get; set;}

        public List<CommitteeForShortpaperDTO>? Committees { get; set;}
    }
}
