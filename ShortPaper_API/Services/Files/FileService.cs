using ShortPaper_API.Entities;
using Microsoft.AspNetCore.Hosting;
using ShortPaper_API.Helper;
using ShortPaper_API.DTO;

namespace ShortPaper_API.Services.Files
{
    public class FileService : IFileService
    {
        private readonly ShortpaperDbContext _db;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public FileService(ShortpaperDbContext db, IWebHostEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
        }

        public IEnumerable<ShortpaperFile> ListFiles(int shortpaperId)
        {
            return _db.ShortpaperFiles
                .Where(file => file.ShortpaperId == shortpaperId)
                .ToList();
        }

        public ShortpaperFile UploadFile(int shortpaperId, IFormFile file, string explanationVideo, string remark, int fileTypeId)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine("wwwroot", "uploads", uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            byte[] fileData;
            using (var memoryStream = new MemoryStream())
            {
                file.OpenReadStream().CopyTo(memoryStream);
                fileData = memoryStream.ToArray();
            }

            var newFile = new ShortpaperFile
            {
                FileName = uniqueFileName,
                FileSize = file.Length.ToString(),
                FileType = file.ContentType,
                ExplanationVideo = explanationVideo,
                ShortpaperFileTypeId = fileTypeId,
                Remark = remark,
                CreatedDatetime = DateTime.Now,
                UpdatedDatetime = DateTime.Now,
                ShortpaperId = shortpaperId
            };

            _db.ShortpaperFiles.Add(newFile);
            _db.SaveChanges();

            return newFile;
        }

        public byte[] DownloadFile(int fileId)
        {
            var file = _db.ShortpaperFiles.FirstOrDefault(f => f.ShortpaperFileId == fileId);

            if (file == null)
            {
                return null;
            }

            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", file.FileName);

            var fileBytes = System.IO.File.ReadAllBytes(filePath);

            return fileBytes;
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
                                    Status = a.Status,
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
                                ShortpaperFileType = new ShortpaperFileTypeDTO
                                {
                                    TypeId = fileType.TypeId,
                                    TypeName = fileType.TypeName,
                                    Status = fileType.Status
                                },
                                Status = fileType.Status

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
                                ShortpaperFileType = new ShortpaperFileTypeDTO
                                {
                                    TypeId = fileType.TypeId,
                                    TypeName = fileType.TypeName,
                                    Status = fileType.Status
                                },
                                Status = fileType.Status

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

        public ServiceResponse<List<ShortpaperFileDTO>> GetFileByIdAndStudent(int fileId, string studentId)
        {
            try
            {
                var file = new List<ShortpaperFileDTO>();

                if (fileId == null || studentId == "" || studentId == null)
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
                                ShortpaperFileType = new ShortpaperFileTypeDTO
                                {
                                    TypeId = fileType.TypeId,
                                    TypeName = fileType.TypeName,
                                    Status = fileType.Status
                                },
                                Status = fileType.Status

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
                            where (shortpaperFileAndStudent.StudentId.Contains(studentId) && a.ShortpaperFileId.Equals(fileId))
                            select new ShortpaperFileDTO
                            {
                                ShortpaperFileId = a.ShortpaperFileId,
                                FileName = a.FileName,
                                UpdatedDatetime = a.UpdatedDatetime,
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
    }
}
