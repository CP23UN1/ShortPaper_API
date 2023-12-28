namespace ShortPaper_API.DTO
{
    public class CommitteeForShortpaperDTO
    {
        public int CommitteeId { get; set; }

        public string Firstname { get; set; } = null!;

        public string Lastname { get; set; } = null!;

        public string Email { get; set; } = null!;
    }
}
