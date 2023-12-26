namespace ShortPaper_API.DTO
{
    public class CreateStudentDTO
    {
        public string StudentId { get; set; } = null!;

        public string Firstname { get; set; } = null!;

        public string Lastname { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? Password { get; set; }

        public string? AlternativeEmail { get; set; }

        public string? Phonenumber { get; set; }

        //public virtual ICollection<Shortpaper> Shortpapers { get; set; } = new List<Shortpaper>();
    }
}
