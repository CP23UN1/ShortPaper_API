using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShortPaper_API.Entities;
using ShortPaper_API.Repositories;
using ShortPaper_API.Services.Users;

namespace ShortPaper_API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ShortpaperDbContext _dbContext;
        private readonly IUserService _userService;

        public UsersController(ShortpaperDbContext dbContext, IUserService userService)
        
        {
            _dbContext = dbContext;
            _userService = userService;
        }

        [HttpGet]
        public List<User> GetUsers()
        {
            var users = _userService.GetUsers();
            return users;
        }

        [HttpGet("{id}")]
        public User GetUserById(int id)
        {
            var user = _userService.GetUser(id);
            return user;
        }

        [HttpPost("create")]
        public User CreateUser(User newUser)
        {
            /*var newUser = new User
            {
                StudentId = "63130500120",
                Firstname = "Siriwat",
                Lastname = "Jai",
                Email = "aom@gmail.com",
                PhoneNumber = "0812345678",
                Year = "2020",
                RegisteredSubjectid = 1,
                ShortpaperSubjectid = 1,
            };*/

            var createUser = _userService.CreateUser(newUser);
            return newUser;
        }

        [HttpPut("update")]
        public User UpdateUser(User user)
        {
            var updateUser = _userService.UpdateUser(user);
            return updateUser;
        }

        [HttpDelete("delete")]
        public int DeleteUser(int id)
        {
            var deleteUser = _userService.DeleteUser(id);
            return deleteUser;
        }
    }
}
