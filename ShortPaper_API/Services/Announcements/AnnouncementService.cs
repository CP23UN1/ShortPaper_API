using ShortPaper_API.DTO;
using ShortPaper_API.Entities;
using ShortPaper_API.Helper;
using System.Globalization;
using System.Net;
using System.Net.Mail;

namespace ShortPaper_API.Services.Announcements
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly ShortpaperDbContext _db;

        public AnnouncementService(ShortpaperDbContext db)
        {
            _db = db;
        }

        public ServiceResponse<List<AnnouncementDTO>> GetAnnouncements()
        {
            try
            {
                var announcements = (from a in _db.Announcements
                                     select new AnnouncementDTO
                                     {
                                         AnnouncementId = a.AnnouncementId,
                                         Schedule = a.Schedule,
                                         Content = a.Content,
                                         CreatedDatetime = a.CreatedDatetime,
                                         ExpiredDatetime = a.ExpiredDatetime,
                                         ImageUrl = a.ImageUrl
                                     }).ToList();

                if (announcements.Count == 0)
                {
                    var noresult = new ServiceResponse<List<AnnouncementDTO>>()
                    {
                        httpStatusCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "No announcements found."
                    };

                    return noresult;
                }

                // Convert date and time to Thai format
                var thaiCulture = new CultureInfo("th-TH");

                var result = new ServiceResponse<List<AnnouncementDTO>>()
                {
                    httpStatusCode = StatusCodes.Status200OK,
                    Data = announcements
                };

                return result;

            }
            catch (Exception ex)
            {

                var result = new ServiceResponse<List<AnnouncementDTO>>()
                {
                    httpStatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = ex.Message
                };

                return result;
            }
        }

        public ServiceResponse<AnnouncementDTO> CreateAnnouncement(AnnouncementDTO newAnnouncement)
        {
            try
            {
                var newAnnounce = new Announcement
                {
                    Schedule = newAnnouncement.Schedule,
                    Content = newAnnouncement.Content,
                    CreatedDatetime = DateTime.Now,
                    ExpiredDatetime = newAnnouncement.ExpiredDatetime,
                    ImageUrl = newAnnouncement.ImageUrl
                };

                _db.Announcements.Add(newAnnounce);

                _db.SaveChanges();

                // Notify all students about the new announcement via email
                NotifyAllStudentsByEmail(newAnnouncement);

                var result = new ServiceResponse<AnnouncementDTO>()
                {
                    httpStatusCode = StatusCodes.Status201Created,
                    Data = newAnnouncement,
                };

                return result;
            }
            catch (Exception ex)
            {
                var result = new ServiceResponse<AnnouncementDTO>()
                {
                    httpStatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = ex.Message
                };

                return result;
            }
        }

        public ServiceResponse<AnnouncementDTO> UpdateAnnouncement(AnnouncementDTO updatedAnnouncement, int id)
        {
            try
            {
                var oldAnnounce = (from a in _db.Announcements
                                   where a.AnnouncementId == id
                                   select a).FirstOrDefault();

                oldAnnounce.Schedule = updatedAnnouncement.Schedule;
                oldAnnounce.Content = updatedAnnouncement.Content;
                oldAnnounce.ExpiredDatetime = updatedAnnouncement.ExpiredDatetime;
                oldAnnounce.ImageUrl = updatedAnnouncement.ImageUrl;

                _db.SaveChanges();

                var result = new ServiceResponse<AnnouncementDTO>()
                {
                    httpStatusCode = StatusCodes.Status200OK,
                    Data = updatedAnnouncement
                };

                return result;
            }
            catch (Exception ex)
            {
                var result = new ServiceResponse<AnnouncementDTO>()
                {
                    httpStatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = ex.Message
                };

                return result;
            }
        }

        public ServiceResponse<Announcement> DeleteAnnouncement(int deletedId)
        {
            try
            {
                var deleteAnnounce = (from a in _db.Announcements
                                      where a.AnnouncementId == deletedId
                                      select a).FirstOrDefault();

                _db.Announcements.Remove(deleteAnnounce);

                _db.SaveChanges();

                var result = new ServiceResponse<Announcement>()
                {
                    httpStatusCode = StatusCodes.Status200OK,
                    Data = deleteAnnounce
                };

                return result;
            }
            catch (Exception ex)
            {
                var result = new ServiceResponse<Announcement>()
                {
                    httpStatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = ex.Message
                };

                return result;
            }
        }

        private List<Student> GetAllStudents()
        {
            // Implement the logic to retrieve all students from your database.
            // Replace this with your actual logic.
            return _db.Students.ToList();
        }

        private void NotifyAllStudentsByEmail(AnnouncementDTO announcement)
        {
            var allStudents = GetAllStudents();

            foreach (var student in allStudents)
            {
                SendEmailToStudent(student, announcement);
            }
        }


        private void SendEmailToStudent(Student student, AnnouncementDTO announcement)
        {
            // Replace these values with your SMTP server credentials and email content.
            string smtpServer = "smtp.gmail.com";
            int smtpPort = 587; // Update with your SMTP port
            string smtpUsername = "oasipssi4@gmail.com";
            string smtpPassword = "nxrpstsxvpyhbkpi";

            string senderEmail = "oasipssi4@gmail.com";
            string subject = "New Announcement";
            string body = $"Dear {student.Firstname},\n\nA new announcement has been created: {announcement.Content}\n\nThank you.";

            try
            {
                using (SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort))
                {
                    smtpClient.EnableSsl = true;
                    smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);

                    MailMessage mailMessage = new MailMessage(senderEmail, student.Email, subject, body);
                    mailMessage.IsBodyHtml = false;

                    smtpClient.Send(mailMessage);
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions here, such as logging the error
                Console.WriteLine($"Failed to send email to {student.Firstname}: {ex.Message}");
            }
        }
    }
}
