using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShortPaper_API.DTO;
using ShortPaper_API.Entities;
using ShortPaper_API.Services.Students;

namespace ShortPaper_API.Controllers
{
    [Route("api")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ShortpaperDbContext _dbContext;
        private readonly IStudentService _studentService;

        public StudentController(ShortpaperDbContext dbContext, IStudentService studentService)

        {
            _dbContext = dbContext;
            _studentService = studentService;
        }

        [HttpGet]
        [Route("students")]
        public List<StudentDTO> GetStudents()
        {
            var students = _studentService.GetStudents();
            return students;
        }

        [HttpGet]
        [Route("student/{id}")]
        public StudentDTO GetStudentById(string id)
        {
            var student = _studentService.GetStudent(id);
            return student;
        }

        [HttpPost]
        [Route("student/create")]
        public ServiceResponse<CreateStudentDTO> CreateStudent(CreateStudentDTO newStudent)
        {
            var status = _studentService.CreateStudent(newStudent);
            return status;
        }

        [HttpPatch]
        [Route("student/update/{id}")]
        public ServiceResponse<UpdateStudentDTO> UpdateUserForStudent(UpdateStudentDTO student)
        {
            var status = _studentService.UpdateStudent(student);
            return status;
        }

        [HttpDelete]
        [Route("student/delete/{id}")]
        public Student DeleteUser(string id)
        {
            var deleteUser = _studentService.DeleteStudent(id);
            return deleteUser;
        }
    }
}
