using ShortPaper_API.Entities;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
using ShortPaper_API.Repositories;

namespace ShortPaper_API.Services.Users
{
    public class UserService : IUserService
    {
        private readonly ShortpaperDbContext _db;

        public UserService(ShortpaperDbContext db) 
        {
            _db = db;
        }

        public User GetUser(int id)
        {
            var user = (from a in _db.Users
                        where a.UserId == id
                        select a).FirstOrDefault();

            return user;
        }

        public List<User> GetUsers()
        {
            var listOfUser = (from a in _db.Users
                          select a).ToList();

            return listOfUser;
        }

        public User CreateUser(User newUser)
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

            return userEntity;
        }

        public User UpdateUser(User user)
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

            return updateUser;
        }
        public int DeleteUser(int id)
        {
            var deleteUser = (from a in _db.Users
                              where a.UserId == id
                              select a).FirstOrDefault();

            _db.Users.Remove(deleteUser);

            _db.SaveChanges();

            return id;
        }
    }
}
