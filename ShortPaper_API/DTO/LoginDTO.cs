using System.ComponentModel.DataAnnotations;

namespace ShortPaper_API.DTO
{
    public class LoginDTO
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
