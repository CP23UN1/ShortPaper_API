using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShortPaper_API.DTO;
using ShortPaper_API.Entities;
using ShortPaper_API.Helper;
using ShortPaper_API.Services.Students;

namespace ShortPaper_API.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        public ServiceResponse<List<StudentDTO>> GetStudents()
        {
            var students = _studentService.GetStudents();
            return students;
        }

        [HttpGet]
        [Route("student/search-by-filter/{filterText}")]
        public ServiceResponse<List<StudentDTO>> GetStudentByFilter(string filterText)
        {
            var students = _studentService.GetStudentByFilter(filterText);
            return students;
        }

        [HttpGet]
        [Route("student/search-by-id/{id}")]
        public ServiceResponse<StudentDTO> GetStudentById(string id)
        {
            var student = _studentService.GetStudent(id);
            return student;
        }

        [HttpGet]
        [Route("student/committee/{id}")]
        public ServiceResponse<List<StudentDTO>> GetStudentByCommittee(string id)
        {
            var student = _studentService.GetStudentByCommittee(id);
            return student;
        }

        [HttpGet]
        [Route("student/committee-filter/{id}/{filterText}")]
        public ServiceResponse<List<StudentDTO>> GetStudentByCommitteeAndFilter(string id, string filterText)
        {
            var student = _studentService.GetStudentByCommitteeAndFilter(id, filterText);
            return student;
        }

        [HttpPost]
        [Route("student/create")]
        [AllowAnonymous]
        public ServiceResponse<CreateStudentDTO> CreateStudent(CreateStudentDTO newStudent)
        {
            var status = _studentService.CreateStudent(newStudent);
            return status;
        }

        [HttpPost]
        [Route("student/add-from-csv")]
        public async Task<IActionResult> AddStudentFromCsv(IFormFile csvFile)
        {
            var result = await _studentService.AddStudentsFromCsvAsync(csvFile);

            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.ErrorMessage);
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

        [HttpGet]
        [Route("years/list")]
        public ServiceResponse<List<string>> GetUniqueYears()
        {
            var response = _studentService.GetUniqueYears();
            return response;
        }
    }
}
