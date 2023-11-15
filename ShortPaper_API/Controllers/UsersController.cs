using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShortPaper_API.Entities;
using ShortPaper_API.Services.Users;

namespace ShortPaper_API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ShortpaperDbContext _dbContext;

        public UsersController(ShortpaperDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public List<User> GetUsers()
        {
            UserService userService = new UserService();
            return userService.GetUsers();
        }

        [HttpGet("{id}")]
        public User GetUserById(int id)
        {
            UserService userService = new UserService();
            return userService.GetUser(id);
        }
    }
}
