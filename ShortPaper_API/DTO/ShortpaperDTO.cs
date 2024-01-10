using ShortPaper_API.Entities;

namespace ShortPaper_API.DTO
{
    public class ShortpaperDTO
    {
        public int ShortpaperId { get; set; }

        public string? ShortpaperTopic { get; set; }

        public SubjectDTO? Subject { get; set; }

        public StudentForShortpaperDTO? StudentForShortpaper { get; set;}

        public CommitteeForShortpaperDTO? CommitteeForShortpaper { get; set;}
    }
}
