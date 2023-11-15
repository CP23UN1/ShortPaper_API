using ShortPaper_API.Entities;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ShortPaper_API.Services.Users
{
    public class UserService : IUserService
    {
        private ShortpaperDbContext _db;

        public UserService() 
        {
            _db = new ShortpaperDbContext();
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
    }
}
