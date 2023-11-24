using ShortPaper_API.Entities;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
using ShortPaper_API.Repositories;
using ShortPaper_API.DTO;

namespace ShortPaper_API.Services.Users
{
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
                         into userRegist from regist in userRegist.DefaultIfEmpty()
                         join c in _db.Subjects on a.ShortpaperSubjectid equals c.Id
                         into userPaper from paper in userPaper.DefaultIfEmpty()
                         where !a.Role.Contains("student")
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
                             ? new Subject
                             {
                                 Id = regist.Id,
                                 SubjectId = regist.SubjectId,
                                 SubjectName = regist.SubjectName
                             } : null,
                             ShortpaperSubject = paper != null
                             ? new Subject
                             {
                                 Id = paper.Id,
                                 SubjectId = paper.SubjectId,
                                 SubjectName = paper.SubjectName
                             } : null
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
                             ? new Subject
                             {
                                 Id = regist.Id,
                                 SubjectId = regist.SubjectId,
                                 SubjectName = regist.SubjectName
                             } : null,
                                ShortpaperSubject = paper != null
                             ? new Subject
                             {
                                 Id = paper.Id,
                                 SubjectId = paper.SubjectId,
                                 SubjectName = paper.SubjectName
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
                               RegisteredSubject = regist != null
                               ? new Subject
                               {
                                   Id = regist.Id,
                                   SubjectId = regist.SubjectId,
                                   SubjectName = regist.SubjectName
                               } : null,
                               ShortpaperSubject = paper != null
                               ? new Subject
                               {
                                   Id = paper.Id,
                                   SubjectId = paper.SubjectId,
                                   SubjectName = paper.SubjectName
                               } : null
                           }).FirstOrDefault();

            return student;
        }

        public UserDTO GetUser(int id)
        {
            var user = (from a in _db.Users
                        where a.UserId == id && !a.Role.Contains("student")
                        select new UserDTO
                        {
                            UserId = a.UserId,
                            Firstname = a.Firstname,
                            Lastname = a.Lastname,
                            Email = a.Email,
                            PhoneNumber = a.PhoneNumber,
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

        public UserDTO UpdateUserForStudent(UserDTO user)
        {
            var updateUser = (from a in _db.Users
                              where a.UserId == user.UserId && a.Role.Contains("student")
                              select a).FirstOrDefault();

            updateUser.Firstname = user.Firstname;
            updateUser.Lastname = user.Lastname;
            updateUser.Email = user.Email;
            updateUser.PhoneNumber = user.PhoneNumber;

            _db.SaveChanges();

            return user;
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
