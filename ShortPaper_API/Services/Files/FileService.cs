using ShortPaper_API.Entities;
using Microsoft.AspNetCore.Hosting;
using ShortPaper_API.Helper;
using ShortPaper_API.DTO;
using ShortPaper_API.Services.Shortpapers;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using PdfiumViewer;
using System.Net;
using System.Net.Mail;
using static System.Net.WebRequestMethods;
using System;
using System.Collections.Generic;

namespace ShortPaper_API.Services.Files
{
    public class FileService : IFileService
    {
        private readonly ShortpaperDbContext _db;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private IShortpaperService _shortpaperService;
        private readonly FileStoreService _fileStoreService;

        public FileService(ShortpaperDbContext db, IWebHostEnvironment hostingEnvironment, IShortpaperService shortpaperService, FileStoreService fileStoreService)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
            _shortpaperService = shortpaperService;
            _fileStoreService = fileStoreService;
        }

        public IEnumerable<ShortpaperFile> ListFiles(int shortpaperId)
        {
            return _db.ShortpaperFiles
                .Where(file => file.ShortpaperId == shortpaperId)
                .ToList();
        }

        public ShortpaperFile UploadFile(int shortpaperId, IFormFile file, string explanationVideo, string remark, int fileTypeId, string studentId)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            if (shortpaperId == 0)
            {
                var shortpaper = new AddShortpaperDTO
                {
                    ShortpaperTopic = null,
                    StudentId = studentId
                };
                _shortpaperService.AddShortpaper(shortpaper);
                _db.SaveChanges();

            }

            var lastshortpaper = _db.Shortpapers.FirstOrDefault(s => s.StudentId == studentId);
            //var shortpaper_has_commitee = _db.ShortpapersHasCommittees.FirstOrDefault(s => s.ShortpaperId == lastshortpaper.ShortpaperId);
            //var student = _db.Students.FirstOrDefault(s => s.StudentId == studentId);
            //var committee = _db.Committees.FirstOrDefault(s => s.CommitteeId == shortpaper_has_commitee.CommitteeId);

            var newFile = new ShortpaperFile
            {
                FileName = file.FileName,
                FileSize = file.Length.ToString(),
                FileType = file.ContentType,
                ExplanationVideo = explanationVideo,
                ShortpaperFileTypeId = fileTypeId,
                Remark = remark,
                CreatedDatetime = DateTime.Now,
                UpdatedDatetime = DateTime.Now,
                ShortpaperId = lastshortpaper.ShortpaperId,
                Status = "not_approve"
            };

            //SendEmailToCommitee(committee,student.Firstname); 

            _db.ShortpaperFiles.Add(newFile);
            _db.SaveChanges();


            // Use FileStoreService to store file
            // Use FileStoreService to store file
            using (var fileStream = file.OpenReadStream())
            {
                _fileStoreService.StoreFile(newFile.FileName, fileStream);
            }
            return newFile;
        }

        public byte[] DownloadFile(int shortpaperId, int fileTypeId)
        {
            // Retrieve the latest file of the specified file type for the given short paper
            var latestFile = _db.ShortpaperFiles
                .Where(f => f.ShortpaperId == shortpaperId && f.ShortpaperFileTypeId == fileTypeId)
                .OrderByDescending(f => f.CreatedDatetime) // Order by upload timestamp descending to get the latest file
                .FirstOrDefault();

            if (latestFile == null)
            {
                return null;
            }

            // Use FileStoreService to retrieve file content
            using (var fileStream = _fileStoreService.GetFile(latestFile.FileName))
            {
                if (fileStream == null)
                {
                    return null;
                }

                using (var memoryStream = new MemoryStream())
                {
                    fileStream.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }

        public ServiceResponse<List<ShortpaperFileTypeDTO>> GetFileType()
        {
            try
            {
                var filetype = (from a in _db.ShortpaperFileTypes
                                select new ShortpaperFileTypeDTO
                                {
                                    TypeId = a.TypeId,
                                    TypeName = a.TypeName,
                                }).ToList();

                var result = new ServiceResponse<List<ShortpaperFileTypeDTO>>
                {
                    Data = filetype,
                    httpStatusCode = StatusCodes.Status200OK
                };

                return result;
            }
            catch (Exception ex)
            {
                var result = new ServiceResponse<List<ShortpaperFileTypeDTO>>()
                {
                    httpStatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = ex.Message
                };

                return result;
            }

        }

        public ServiceResponse<List<ShortpaperFileDTO>> GetFiles()
        {
            try
            {
                var file = (from a in _db.ShortpaperFiles
                            join b in _db.ShortpaperFileTypes on a.ShortpaperFileTypeId equals b.TypeId
                            into ft
                            from fileType in ft.DefaultIfEmpty()

                            select new ShortpaperFileDTO
                            {
                                ShortpaperFileId = a.ShortpaperFileId,
                                FileName = a.FileName,
                                UpdatedDatetime = a.UpdatedDatetime,
                                Status = a.Status ?? "not_send",
                                ShortpaperFileType = new ShortpaperFileTypeDTO
                                {
                                    TypeId = fileType.TypeId,
                                    TypeName = fileType.TypeName,
                                },
                            }).ToList();

                var result = new ServiceResponse<List<ShortpaperFileDTO>>
                {
                    Data = file,
                    httpStatusCode = StatusCodes.Status200OK,
                };

                return result;
            }
            catch (Exception ex)
            {
                var result = new ServiceResponse<List<ShortpaperFileDTO>>()
                {
                    httpStatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = ex.Message
                };

                return result;
            }
        }

        public ServiceResponse<List<ShortpaperFileDTO>> GetFileByStudent(string id)
        {
            try
            {
                var file = new List<ShortpaperFileDTO>();

                if (id == "" || id == null)
                {
                    file = (from a in _db.ShortpaperFiles

                            join b in _db.ShortpaperFileTypes on a.ShortpaperFileTypeId equals b.TypeId
                            into ft
                            from fileType in ft.DefaultIfEmpty()
                            select new ShortpaperFileDTO
                            {
                                ShortpaperFileId = a.ShortpaperFileId,
                                FileName = a.FileName,
                                UpdatedDatetime = a.UpdatedDatetime,
                                Status = a.Status ?? "not_send",
                                ShortpaperFileType = new ShortpaperFileTypeDTO
                                {
                                    TypeId = fileType.TypeId,
                                    TypeName = fileType.TypeName,
                                },
                            }).ToList();

                    var result = new ServiceResponse<List<ShortpaperFileDTO>>
                    {
                        Data = file,
                        httpStatusCode = StatusCodes.Status200OK,
                    };

                    return result;
                }
                else
                {
                    file = (from a in _db.ShortpaperFiles

                            join b in _db.ShortpaperFileTypes on a.ShortpaperFileTypeId equals b.TypeId
                            into ft
                            from fileType in ft.DefaultIfEmpty()
                            join c in _db.Shortpapers on a.ShortpaperId equals c.ShortpaperId
                            into ft2
                            from shortpaperFileAndStudent in ft2.DefaultIfEmpty()
                            where shortpaperFileAndStudent.StudentId.Contains(id)
                            select new ShortpaperFileDTO
                            {
                                ShortpaperFileId = a.ShortpaperFileId,
                                FileName = a.FileName,
                                UpdatedDatetime = a.UpdatedDatetime,
                                Status = a.Status ?? "not_send",
                                ShortpaperFileType = new ShortpaperFileTypeDTO
                                {
                                    TypeId = fileType.TypeId,
                                    TypeName = fileType.TypeName,
                                }

                            }).ToList();

                    var result = new ServiceResponse<List<ShortpaperFileDTO>>
                    {
                        Data = file,
                        httpStatusCode = StatusCodes.Status200OK,
                    };

                    return result;
                }
            }
            catch (Exception ex)
            {
                var result = new ServiceResponse<List<ShortpaperFileDTO>>()
                {
                    httpStatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = ex.Message
                };

                return result;
            }
        }

        public ServiceResponse<ShortpaperFileDTO> GetFileByIdAndStudent(int fileId, string studentId)
        {
            try
            {
                var latestFile = (from a in _db.ShortpaperFiles
                                  join b in _db.ShortpaperFileTypes on a.ShortpaperFileTypeId equals b.TypeId
                                  into ft
                                  from fileType in ft.DefaultIfEmpty()
                                  join c in _db.Shortpapers on a.ShortpaperId equals c.ShortpaperId
                                  into ft2
                                  from shortpaperFileAndStudent in ft2.DefaultIfEmpty()
                                  where shortpaperFileAndStudent.StudentId == studentId
                                  orderby a.CreatedDatetime descending
                                  select new ShortpaperFileDTO
                                  {
                                      ShortpaperFileId = a.ShortpaperFileId,
                                      FileName = a.FileName,
                                      UpdatedDatetime = a.UpdatedDatetime,
                                      Status = a.Status ?? "not_send",
                                      ShortpaperFileType = new ShortpaperFileTypeDTO
                                      {
                                          TypeId = fileType.TypeId,
                                          TypeName = fileType.TypeName,
                                      }

                                  }).FirstOrDefault();

                if (latestFile == null)
                {
                    return new ServiceResponse<ShortpaperFileDTO>
                    {
                        httpStatusCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "No files found for the specified student."
                    };
                }

                var result = new ServiceResponse<ShortpaperFileDTO>
                {
                    Data = latestFile,
                    httpStatusCode = StatusCodes.Status200OK,
                };

                return result;
            }
            catch (Exception ex)
            {
                var result = new ServiceResponse<ShortpaperFileDTO>()
                {
                    httpStatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = ex.Message
                };

                return result;
            }
        }

        public ServiceResponse<ShortpaperFileDTO> GetFileByTypeIdAndShortpaper(int fileTypeId, int shortpaperId)
        {
            try
            {
                var latestFile = (from a in _db.ShortpaperFiles
                                  join b in _db.ShortpaperFileTypes on fileTypeId equals b.TypeId
                                  into ft
                                  from fileType in  ft.DefaultIfEmpty()
                                  join c in _db.Shortpapers on a.ShortpaperId equals c.ShortpaperId
                                  into ft2
                                  from shortpaperFileAndStudent in ft2.DefaultIfEmpty()
                                  where a.ShortpaperId == shortpaperId && a.ShortpaperFileTypeId == fileTypeId
                                  orderby a.CreatedDatetime descending
                                  select new ShortpaperFileDTO
                                  {
                                      ShortpaperFileId = a.ShortpaperFileId,
                                      FileName = a.FileName,
                                      UpdatedDatetime = a.UpdatedDatetime,
                                      Status = a.Status ?? "not_send",
                                      ShortpaperFileType = new ShortpaperFileTypeDTO
                                      {
                                          TypeId = fileType.TypeId,
                                          TypeName = fileType.TypeName,
                                      }

                                  }).FirstOrDefault();

                if (latestFile == null)
                {
                    return new ServiceResponse<ShortpaperFileDTO>
                    {
                        httpStatusCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "No files found for the specified student."
                    };
                }

                var result = new ServiceResponse<ShortpaperFileDTO>
                {
                    Data = latestFile,
                    httpStatusCode = StatusCodes.Status200OK,
                };

                return result;
            }
            catch (Exception ex)
            {
                var result = new ServiceResponse<ShortpaperFileDTO>()
                {
                    httpStatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = ex.Message
                };

                return result;
            }
        }


        public ServiceResponse<List<ShortpaperFileDTO>> GetFileByCommittee(string committeeId)
        {
            try
            {
                var file = new List<ShortpaperFileDTO>();

                var shasc = _db.ShortpapersHasCommittees.FirstOrDefault(s => s.CommitteeId.Contains(committeeId));
                var shortpaper = _db.Shortpapers.FirstOrDefault(s => s.ShortpaperId == shasc.ShortpaperId);
                var student = _db.Students.FirstOrDefault(s => s.StudentId == shortpaper.StudentId);

                if (committeeId == null)
                {
                    file = (from a in _db.ShortpaperFiles

                            join b in _db.ShortpaperFileTypes on a.ShortpaperFileTypeId equals b.TypeId
                            into ft
                            from fileType in ft.DefaultIfEmpty()
                            select new ShortpaperFileDTO
                            {
                                ShortpaperFileId = a.ShortpaperFileId,
                                FileName = a.FileName,
                                UpdatedDatetime = a.UpdatedDatetime,
                                Status = a.Status,
                                ShortpaperFileType = new ShortpaperFileTypeDTO
                                {
                                    TypeId = fileType.TypeId,
                                    TypeName = fileType.TypeName,
                                },

                            }).ToList();

                    var result = new ServiceResponse<List<ShortpaperFileDTO>>
                    {
                        Data = file,
                        httpStatusCode = StatusCodes.Status200OK,
                    };

                    return result;
                }
                else
                {
                    file = (from a in _db.ShortpaperFiles
                            join b in _db.ShortpaperFileTypes on a.ShortpaperFileTypeId equals b.TypeId
                            into ft
                            from fileType in ft.DefaultIfEmpty()
                            join c in _db.Shortpapers on a.ShortpaperId equals c.ShortpaperId
                            into ft2
                            from shortpaperFileAndStudent in ft2.DefaultIfEmpty()
                            where (shortpaperFileAndStudent.StudentId.Contains(student.StudentId))
                            select new ShortpaperFileDTO
                            {
                                ShortpaperFileId = a.ShortpaperFileId,
                                FileName = a.FileName,
                                UpdatedDatetime = a.UpdatedDatetime,
                                Status = a.Status,
                                ShortpaperFileType = new ShortpaperFileTypeDTO
                                {
                                    TypeId = fileType.TypeId,
                                    TypeName = fileType.TypeName,
                                }

                            }).ToList();

                    var result = new ServiceResponse<List<ShortpaperFileDTO>>
                    {
                        Data = file,
                        httpStatusCode = StatusCodes.Status200OK,
                    };

                    return result;
                }
            }
            catch (Exception ex)
            {
                var result = new ServiceResponse<List<ShortpaperFileDTO>>()
                {
                    httpStatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = ex.Message
                };

                return result;
            }
        }

        public byte[] GeneratePdfPreview(int fileId)
        {
            var file = _db.ShortpaperFiles.FirstOrDefault(f => f.ShortpaperFileId == fileId);

            if (file == null || file.FileType != "application/pdf")
            {
                return null; // Not a PDF file or file not found
            }

            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", file.FileName);

            using (var document = PdfDocument.Load(filePath))
            {
                // Render the first page of the PDF as an image
                using (var image = document.Render(0, 300, 300, true))
                {
                    // Convert the rendered image to a byte array
                    using (var stream = new MemoryStream())
                    {
                        image.Save(stream, ImageFormat.Jpeg);
                        return stream.ToArray();
                    }
                }
            }
        }

        private void SendEmailToCommitee(Committee committee,string studentName)
        {
            // Replace these values with your SMTP server credentials and email content.
            string smtpServer = "smtp.gmail.com";
            int smtpPort = 587; // Update with your SMTP port
            string smtpUsername = "shortpaper.cp23un1@gmail.com";
            string smtpPassword = "hejXoqfibxan3niqbu";

            string senderEmail = "shortpaper.cp23un1@gmail.com";
            string subject = "File Upload From Student";
            string link = "https://capstone23.sit.kmutt.ac.th/un1/#/login";
            string body = $"Dear {committee.Firstname},\n\nA new file from {studentName} has been uploaded \n\nYou can check in this link: <a href='{link}'>here</a>.";

            try
            {
                using (SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort))
                {
                    smtpClient.EnableSsl = true;
                    smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);

                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress(senderEmail, "Non-Reply Shortpaper System");
                    mailMessage.To.Add(new MailAddress(committee.Email, committee.Firstname));
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = true;

                    // Set the Reply-To header to a non-reply email address
                    mailMessage.ReplyToList.Add("noreply@example.com");

                    smtpClient.Send(mailMessage);
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions here, such as logging the error
                Console.WriteLine($"Failed to send email to {studentName}: {ex.Message}");
            }
        }
    }
}
