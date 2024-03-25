namespace ShortPaper_API.DTO
{
    public class SubjectDTO
    {
        public string SubjectId { get; set; } = null!;

        public string SubjectName { get; set; } = null!;

        public ulong IsRegisteredSubject { get; set; }

        public ulong IsPaperSubject { get; set; }
    }
}
