namespace ShortPaper_API.DTO
{
    public class CommitteeDTO
    {
        public int CommitteeId { get; set; }

        public string Firstname { get; set; } = null!;

        public string Lastname { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? AlternativeEmail { get; set; }


        public string? Phonenumber { get; set; }

        public ulong IsAdvisor { get; set; }
    }
}
