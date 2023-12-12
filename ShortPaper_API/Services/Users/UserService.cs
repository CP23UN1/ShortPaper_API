using ShortPaper_API.Entities;
using System.Data;
using System.Net.Mail;
using System.Linq;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
using ShortPaper_API.Repositories;
using ShortPaper_API.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ShortPaper_API.Services.Users
{
    public class ServiceResponse<T>
    {
        public bool IsSuccess { get; set; }
        public T Data { get; set; }
        public string ErrorMessage { get; set; }
        public int StatusCode { get; set; } = StatusCodes.Status200OK; // Default to 200 OK

        // Other properties or methods as needed
    }

    public class UserService : IUserService
    {
        private readonly ShortpaperDbContext _db;

        public UserService(ShortpaperDbContext db)
        {
            _db = db;
        }

        public List<UserDTO> GetUsers()
        {
            var users = (from a in _db.Users
                         join b in _db.Subjects on a.RegisteredSubjectid equals b.Id
                         into userRegist
                         from regist in userRegist.DefaultIfEmpty()
                         join c in _db.Subjects on a.ShortpaperSubjectid equals c.Id
                         into userPaper
                         from paper in userPaper.DefaultIfEmpty()
                         join d in _db.Projects on a.UserId equals d.StudentId
                         into project
                         from proj in project.DefaultIfEmpty()
                         select new UserDTO
                         {
                             UserId = a.UserId,
                             StudentId = a.StudentId,
                             Firstname = a.Firstname,
                             Lastname = a.Lastname,
                             Role = a.Role,
                             Email = a.Email,
                             PhoneNumber = a.PhoneNumber,
                             Year = a.Year,
                             RegisteredSubject = regist != null
                             ? new SubjectDTO
                             {
                                 Id = regist.Id,
                                 SubjectId = regist.SubjectId,
                                 SubjectName = regist.SubjectName
                             } : null,
                             ShortpaperSubject = paper != null
                             ? new SubjectDTO
                             {
                                 Id = paper.Id,
                                 SubjectId = paper.SubjectId,
                                 SubjectName = paper.SubjectName
                             } : null,
                             ProjectName = proj.Topic
                         }).ToList();

            return users;
        }

        public List<UserDTO> GetStudents()
        {
            var students = (from a in _db.Users
                            join b in _db.Subjects on a.RegisteredSubjectid equals b.Id
                            into userRegist
                            from regist in userRegist.DefaultIfEmpty()
                            join c in _db.Subjects on a.ShortpaperSubjectid equals c.Id
                            into userPaper
                            from paper in userPaper.DefaultIfEmpty()
                            join d in _db.Projects on a.UserId equals d.StudentId
                            into project
                            from proj in project.DefaultIfEmpty()
                            join e in _db.Files on proj.ProjectId equals e.ProjectId
                            into files
                            from file in files.DefaultIfEmpty()
                            join f in _db.FileStatuses on file.StatusId equals f.StatusId
                            into fileStatus
                            from status in fileStatus.DefaultIfEmpty()
                            where a.Role.Contains("student")
                            select new UserDTO
                            {
                                UserId = a.UserId,
                                StudentId = a.StudentId,
                                Firstname = a.Firstname,
                                Lastname = a.Lastname,
                                Role = a.Role,
                                Email = a.Email,
                                PhoneNumber = a.PhoneNumber,
                                Year = a.Year,
                                RegisteredSubject = regist != null
                             ? new SubjectDTO
                             {
                                 Id = regist.Id,
                                 SubjectId = regist.SubjectId,
                                 SubjectName = regist.SubjectName
                             } : null,
                                ShortpaperSubject = paper != null
                             ? new SubjectDTO
                             {
                                 Id = paper.Id,
                                 SubjectId = paper.SubjectId,
                                 SubjectName = paper.SubjectName
                             } : null,
                                ProjectName = proj != null ? proj.Topic : null,
                                FileStatus = status != null
                            ? new FileStatus
                            {
                                StatusId = status.StatusId,
                                BOne = status.BOne,
                                PaperOne = status.PaperOne,
                                PaperTwo = status.PaperTwo,
                                Article = status.Article,
                                Plagiarism = status.Plagiarism,
                                Copyright = status.Copyright,
                                Robbery = status.Robbery,
                                Final = status.Final
                            } : null
                            }).ToList();

            return students;
        }

        public UserDTO GetStudent(int id)
        {
            var student = (from a in _db.Users
                           join b in _db.Subjects on a.RegisteredSubjectid equals b.Id
                            into userRegist
                           from regist in userRegist.DefaultIfEmpty()
                           join c in _db.Subjects on a.ShortpaperSubjectid equals c.Id
                           into userPaper
                           from paper in userPaper.DefaultIfEmpty()
                           join d in _db.Projects on a.UserId equals d.StudentId
                           into project
                           from proj in project.DefaultIfEmpty()
                           join e in _db.Files on proj.ProjectId equals e.ProjectId
                            into files
                           from file in files.DefaultIfEmpty()
                           join f in _db.FileStatuses on file.StatusId equals f.StatusId
                           into fileStatus
                           from status in fileStatus.DefaultIfEmpty()
                           where a.UserId == id && a.Role.Contains("student")
                           select new UserDTO
                           {
                               UserId = a.UserId,
                               StudentId = a.StudentId,
                               Firstname = a.Firstname,
                               Lastname = a.Lastname,
                               Email = a.Email,
                               PhoneNumber = a.PhoneNumber,
                               Year = a.Year,
                               RegisteredSubjectid = regist.Id,
                               ShortpaperSubjectid = paper.Id,
                               ProjectName = proj.Topic,
                               RegisteredSubject = regist != null
                               ? new SubjectDTO
                               {
                                   Id = regist.Id,
                                   SubjectId = regist.SubjectId,
                                   SubjectName = regist.SubjectName
                               } : null,
                               ShortpaperSubject = paper != null
                               ? new SubjectDTO
                               {
                                   Id = paper.Id,
                                   SubjectId = paper.SubjectId,
                                   SubjectName = paper.SubjectName
                               } : null,
                               FileStatus = status != null
                            ? new FileStatus
                            {
                                StatusId = status.StatusId,
                                BOne = status.BOne,
                                PaperOne = status.PaperOne,
                                PaperTwo = status.PaperTwo,
                                Article = status.Article,
                                Plagiarism = status.Plagiarism,
                                Copyright = status.Copyright,
                                Robbery = status.Robbery,
                                Final = status.Final
                            } : null
                           }).FirstOrDefault();

            return student;
        }
        public List<UserDTO> GetAdvisors()
        {
            var advisors = (from a in _db.Users
                            join b in _db.Subjects on a.RegisteredSubjectid equals b.Id
                            into userRegist
                            from regist in userRegist.DefaultIfEmpty()
                            join c in _db.Subjects on a.ShortpaperSubjectid equals c.Id
                            into userPaper
                            from paper in userPaper.DefaultIfEmpty()
                            join d in _db.Projects on a.UserId equals d.StudentId
                            into project
                            from proj in project.DefaultIfEmpty()
                            join e in _db.Files on proj.ProjectId equals e.ProjectId
                            into files
                            from file in files.DefaultIfEmpty()
                            join f in _db.FileStatuses on file.StatusId equals f.StatusId
                            into fileStatus
                            from status in fileStatus.DefaultIfEmpty()
                            where a.Role.Contains("advisor")
                            select new UserDTO
                            {
                                UserId = a.UserId,
                                StudentId = a.StudentId,
                                Firstname = a.Firstname,
                                Lastname = a.Lastname,
                                Email = a.Email,
                                PhoneNumber = a.PhoneNumber,
                                Year = a.Year,
                                RegisteredSubjectid = regist.Id,
                                ShortpaperSubjectid = paper.Id,
                                ProjectName = proj.Topic
                                //RegisteredSubject = regist != null
                                //? new Subject
                                //{
                                //    Id = regist.Id,
                                //    SubjectId = regist.SubjectId,
                                //    SubjectName = regist.SubjectName
                                //} : null,
                                //ShortpaperSubject = paper != null
                                //? new Subject
                                //{
                                //    Id = paper.Id,
                                //    SubjectId = paper.SubjectId,
                                //    SubjectName = paper.SubjectName
                                //} : null,
                            }).ToList();

            return advisors;
        }

        public UserDTO GetAdvisor(int id)
        {
            var advisor = (from a in _db.Users
                           join b in _db.Subjects on a.RegisteredSubjectid equals b.Id
                            into userRegist
                           from regist in userRegist.DefaultIfEmpty()
                           join c in _db.Subjects on a.ShortpaperSubjectid equals c.Id
                           into userPaper
                           from paper in userPaper.DefaultIfEmpty()
                           join d in _db.Projects on a.UserId equals d.StudentId
                           into project
                           from proj in project.DefaultIfEmpty()
                           where a.UserId == id && a.Role.Contains("advisor")
                           select new UserDTO
                           {
                               UserId = a.UserId,
                               StudentId = a.StudentId,
                               Firstname = a.Firstname,
                               Lastname = a.Lastname,
                               Email = a.Email,
                               PhoneNumber = a.PhoneNumber,
                               Year = a.Year,
                               RegisteredSubjectid = regist.Id,
                               ShortpaperSubjectid = paper.Id,
                               ProjectName = proj.Topic
                               //RegisteredSubject = regist != null
                               //? new Subject
                               //{
                               //    Id = regist.Id,
                               //    SubjectId = regist.SubjectId,
                               //    SubjectName = regist.SubjectName
                               //} : null,
                               //ShortpaperSubject = paper != null
                               //? new Subject
                               //{
                               //    Id = paper.Id,
                               //    SubjectId = paper.SubjectId,
                               //    SubjectName = paper.SubjectName
                               //} : null,
                           }).FirstOrDefault();

            return advisor;
        }

        public UserDTO GetUser(int id)
        {
            var user = (from a in _db.Users
                        join b in _db.Subjects on a.RegisteredSubjectid equals b.Id
                        into userRegist
                        from regist in userRegist.DefaultIfEmpty()
                        join c in _db.Subjects on a.ShortpaperSubjectid equals c.Id
                        into userPaper
                        from paper in userPaper.DefaultIfEmpty()
                        join d in _db.Projects on a.UserId equals d.StudentId
                        into project
                        from proj in project.DefaultIfEmpty()
                        where a.UserId == id
                        select new UserDTO
                        {
                            UserId = a.UserId,
                            StudentId = a.StudentId,
                            Firstname = a.Firstname,
                            Lastname = a.Lastname,
                            Email = a.Email,
                            PhoneNumber = a.PhoneNumber,
                            Year = a.Year,
                            RegisteredSubjectid = regist.Id,
                            ShortpaperSubjectid = paper.Id,
                            ProjectName = proj.Topic
                            //RegisteredSubject = regist != null
                            //? new Subject
                            //{
                            //    Id = regist.Id,
                            //    SubjectId = regist.SubjectId,
                            //    SubjectName = regist.SubjectName
                            //} : null,
                            //ShortpaperSubject = paper != null
                            //? new Subject
                            //{
                            //    Id = paper.Id,
                            //    SubjectId = paper.SubjectId,
                            //    SubjectName = paper.SubjectName
                            //} : null,

                        }).FirstOrDefault();

            return user;
        }


        public UserDTO CreateUser(UserDTO newUser)
        {
            var userEntity = new User
            {
                StudentId = newUser.StudentId,
                Firstname = newUser.Firstname,
                Lastname = newUser.Lastname,
                Email = newUser.Email,
                PhoneNumber = newUser.PhoneNumber,
                Year = newUser.Year,
                RegisteredSubjectid = newUser.RegisteredSubjectid,
                ShortpaperSubjectid = newUser.ShortpaperSubjectid
            };

            _db.Users.Add(userEntity);

            _db.SaveChanges();

            return newUser;
        }

        [ProducesResponseType(typeof(ServiceResponse<UserDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResponse<UserDTO>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponse<UserDTO>), StatusCodes.Status500InternalServerError)]
        public ServiceResponse<UserDTO> UpdateUserForStudent(UserDTO user)
        {
            var response = new ServiceResponse<UserDTO>();

            try
            {
                if (string.IsNullOrEmpty(user.Firstname) || string.IsNullOrEmpty(user.Lastname) || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.PhoneNumber))
                {
                    response.ErrorMessage = "Firstname, Lastname, Email, and PhoneNumber are required.";
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    return response;
                }

                if (!IsValidEmail(user.Email))
                {
                    response.ErrorMessage = "Invalid email address";
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    return response;
                }

                if (string.IsNullOrEmpty(user.ProjectName))
                {
                    response.ErrorMessage = "ProjectName is required.";
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    return response;
                }

                var updateUser = (from a in _db.Users
                                  where a.UserId == user.UserId && a.Role.Contains("student")
                                  select a).FirstOrDefault();

                updateUser.Firstname = user.Firstname;
                updateUser.Lastname = user.Lastname;
                updateUser.Email = user.Email;
                updateUser.PhoneNumber = user.PhoneNumber;

                var checkProject = (from b in _db.Projects
                                    where b.StudentId == user.UserId
                                    select b).FirstOrDefault();

                if (checkProject == null)
                {
                    var newProject = new Project
                    {
                        Topic = user.ProjectName,
                        StudentId = user.UserId,
                    };

                    _db.Projects.Add(newProject);
                }
                else
                {
                    checkProject.Topic = user.ProjectName;
                }

                _db.SaveChanges();

                response.IsSuccess = true;
                response.Data = user;
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Exception: {ex.Message}");

                response.ErrorMessage = "An unexpected error occurred";
                response.StatusCode = StatusCodes.Status500InternalServerError;
            }

            return response;
        }


        private bool IsValidEmail(string email){
            try
            {
                var mailAddress = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public UserDTO UpdateUserForAdmin(UserDTO user)
        {
            var updateUser = (from a in _db.Users
                              where a.UserId == user.UserId
                              select a).FirstOrDefault();

            updateUser.StudentId = user.StudentId;
            updateUser.Firstname = user.Firstname;
            updateUser.Lastname = user.Lastname;
            updateUser.Email = user.Email;
            updateUser.PhoneNumber = user.PhoneNumber;
            updateUser.Year = user.Year;
            updateUser.RegisteredSubjectid = user.RegisteredSubjectid;
            updateUser.ShortpaperSubjectid = user.ShortpaperSubjectid;

            _db.SaveChanges();

            return user;
        }

        public User DeleteUser(int id)
        {
            var deleteUser = (from a in _db.Users
                              where a.UserId == id
                              select a).FirstOrDefault();

            _db.Users.Remove(deleteUser);

            _db.SaveChanges();

            return deleteUser;
        }
    }
}
