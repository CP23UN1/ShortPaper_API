using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShortPaper_API.DTO;
using ShortPaper_API.Entities;
using ShortPaper_API.Repositories;
using ShortPaper_API.Services.Users;

namespace ShortPaper_API.Controllers
{
    //[EnableCors("VueCorsPolicy")]
    [Route("api")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ShortpaperDbContext _dbContext;
        private readonly IUserService _userService;

        public UserController(ShortpaperDbContext dbContext, IUserService userService)
        
        {
            _dbContext = dbContext;
            _userService = userService;
        }

        [HttpGet]
        [Route("users")]
        public List<UserDTO> GetUsers()
        {
            var users = _userService.GetUsers();
            return users;
        }

        [HttpGet]
        [Route("students")]
        public List<UserDTO> GetStudents()
        {
            var students = _userService.GetStudents();
            return students;
        }

        [HttpGet]
        [Route("student/{id}")]
        public UserDTO GetStudentById(int id)
        {
            var student = _userService.GetStudent(id);
            return student;
        }

        [HttpGet]
        [Route("advisors")]
        public List<UserDTO> GetAdvisors()
        {
            var students = _userService.GetAdvisors();
            return students;
        }

        [HttpGet]
        [Route("advisor/{id}")]
        public UserDTO GetAdvisorById(int id)
        {
            var student = _userService.GetAdvisor(id);
            return student;
        }

        [HttpGet]
        [Route("user/{id}")]
        public UserDTO GetUserById(int id)
        {
            var user = _userService.GetUser(id);
            return user;
        }

        [HttpPost]
        [Route("user/create")]
        public UserDTO CreateUser(UserDTO newUser)
        {
            var createUser = _userService.CreateUser(newUser);
            return createUser;
        }

        [HttpPut]
        [Route("user/update/student/{id}")]
        public UserDTO UpdateUserForStudent(UserDTO user)
        {
            var updateUser = _userService.UpdateUserForStudent(user);
            return updateUser;
        }

        [HttpPut]
        [Route("user/update/admin/{id}")]
        public UserDTO UpdateUserForAdmin(UserDTO user)
        {
            var updateUser = _userService.UpdateUserForAdmin(user);
            return updateUser;
        }

        [HttpDelete]
        [Route("user/delete/{id}")]
        public User DeleteUser(int id)
        {
            var deleteUser = _userService.DeleteUser(id);
            return deleteUser;
        }
    }
}
