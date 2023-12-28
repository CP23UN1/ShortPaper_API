using ShortPaper_API.Entities;
using System.Globalization;

namespace ShortPaper_API.DTO
{
    public class AnnouncementDTO
    {
        public int AnnouncementId { get; set; }

        public string Schedule { get; set; } = null!;

        public string Content { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public DateTime CreatedDatetime { get; set; }

        public DateTime? ExpiredDatetime { get; set; }

        public string CreatedDatetimeThai => CreatedDatetime.ToString("D", CultureInfo.GetCultureInfo("th-TH"));
        public string ExpiredDatetimeThai => ExpiredDatetime?.ToString("D", CultureInfo.GetCultureInfo("th-TH"));
        //public List<AnnouncementFile>? AnnouncementFiles { get; set; }
    }
}
