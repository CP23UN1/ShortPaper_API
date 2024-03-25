using Konscious.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShortPaper_API.DTO;
using ShortPaper_API.Entities;
using ShortPaper_API.Helper;
using System.Net.Mail;
using System.Text;

namespace ShortPaper_API.Services.Students
{
    public class StudentService : IStudentService
    {
        private readonly ShortpaperDbContext _db;

        public StudentService(ShortpaperDbContext db)
        {
            _db = db;
        }

        public ServiceResponse<List<StudentDTO>> GetStudents()
        {
            try
            {
                var students = (from student in _db.Students
                                join shortpaper in _db.Shortpapers on student.StudentId equals shortpaper.StudentId
                                into studentShortpaper
                                from studentShort in studentShortpaper.DefaultIfEmpty()
                                join h in _db.ShortpapersHasCommittees on studentShort.ShortpaperId equals h.ShortpaperId
                                into shortperHasCommittee
                                from shc in shortperHasCommittee.DefaultIfEmpty()
                                join committee in _db.Committees on shc.CommitteeId equals committee.CommitteeId
                                into committees
                                from c in committees.DefaultIfEmpty()
                                join shs in _db.StudentsHasSubjects on student.StudentId equals shs.StudentId
                                into studentsHasSubjects
                                from sshs in studentsHasSubjects.DefaultIfEmpty()
                                select new
                                {
                                    Student = new StudentDTO
                                    {
                                        StudentId = student.StudentId,
                                        Firstname = student.Firstname,
                                        Lastname = student.Lastname,
                                        Email = student.Email,
                                        AlternativeEmail = student.AlternativeEmail,
                                        Phonenumber = student.Phonenumber,
                                        Year = student.Year,
                                        Shortpaper = studentShort != null ? new ShortpaperForStudentDTO
                                        {
                                            ShortpaperId = studentShort.ShortpaperId,
                                            ShortpaperTopic = studentShort.ShortpaperTopic,
                                        } : null,
                                    },
                                    //Subjects = (from sps in _db.Subjects
                                    //            where sps.SubjectId == studentShort.SubjectId
                                    //            select new SubjectDTO
                                    //            {
                                    //                SubjectId = sps.SubjectId,
                                    //                SubjectName = sps.SubjectName,
                                    //            }).FirstOrDefault(),
                                    Committee = c != null ? new CommitteeDTO
                                    {
                                        CommitteeId = c.CommitteeId,
                                        Firstname = c.Firstname,
                                        Lastname = c.Lastname,
                                        Email = c.Email,
                                        AlternativeEmail = c.AlternativeEmail,
                                        Phonenumber = c.Phonenumber,
                                        IsAdvisor = shc.IsAdvisor,
                                        IsPrincipal = shc.IsPrincipal,
                                        IsCommittee = shc.IsCommittee,
                                    } : null,
                                    ShortpaperFiles = (from spf in _db.ShortpaperFiles
                                                       join type in _db.ShortpaperFileTypes on spf.ShortpaperFileTypeId equals type.TypeId
                                                       into shortpaperFileDetail
                                                       from spfd in shortpaperFileDetail.DefaultIfEmpty()
                                                       where spf.ShortpaperId == studentShort.ShortpaperId
                                                       select new ShortpaperFileForStudentDTO
                                                       {
                                                           ShortpaperFileId = spf.ShortpaperFileId,
                                                           FileName = spf.FileName,
                                                           Status = spf.Status,
                                                           ShortpaperFileTypeId = spf.ShortpaperFileTypeId
                                                       }).ToList(),
                                    Subjects = (from spf in _db.Subjects
                                                where spf.SubjectId == sshs.SubjectId
                                                select new SubjectDTO
                                                {
                                                    SubjectId = spf.SubjectId,
                                                    SubjectName = spf.SubjectName,
                                                    IsRegisteredSubject = sshs.IsRegisteredSubject,
                                                    IsPaperSubject = sshs.IsPaperSubject
                                                }).ToList(),
                                })
                .GroupBy(x => x.Student.StudentId) // Group by StudentId
                .Select(group => new StudentDTO
                {
                    // Select properties from the first item in the group (assuming Student properties are the same for each group)
                    StudentId = group.First().Student.StudentId,
                    Firstname = group.First().Student.Firstname,
                    Lastname = group.First().Student.Lastname,
                    Email = group.First().Student.Email,
                    AlternativeEmail = group.First().Student.AlternativeEmail,
                    Phonenumber = group.First().Student.Phonenumber,
                    Year = group.First().Student.Year,
                    Shortpaper = group.First().Student.Shortpaper,
                    //Subjects = group.First().Subjects,
                    Subjects = group.First().Subjects,
                    Committees = group.Select(x => x.Committee).ToList(),
                    ShortpaperFiles = group.First().ShortpaperFiles
                })
                .ToList();


                if (students.Count == 0)
                {
                    var noresult = new ServiceResponse<List<StudentDTO>>()
                    {
                        httpStatusCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "no students found."
                    };
                    return noresult;
                }
                else
                {
                    var result = new ServiceResponse<List<StudentDTO>>()
                    {
                        httpStatusCode = StatusCodes.Status200OK,
                        Data = students
                    };

                    return result;
                }
            }
            catch (Exception ex)
            {

                var result = new ServiceResponse<List<StudentDTO>>()
                {
                    httpStatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = ex.Message
                };

                return result;
            }
        }

        public ServiceResponse<List<StudentDTO>> GetStudentByFilter(string searchText)
        {
            try
            {
                var students = new List<StudentDTO>();

                if (searchText == "" || searchText == null)
                {
                         students = (from student in _db.Students
                                     join shortpaper in _db.Shortpapers on student.StudentId equals shortpaper.StudentId
                                     into studentShortpaper
                                     from studentShort in studentShortpaper.DefaultIfEmpty()
                                     join h in _db.ShortpapersHasCommittees on studentShort.ShortpaperId equals h.ShortpaperId
                                     into shortperHasCommittee
                                     from shc in shortperHasCommittee.DefaultIfEmpty()
                                     join committee in _db.Committees on shc.CommitteeId equals committee.CommitteeId
                                     into committees
                                     from c in committees.DefaultIfEmpty()
                                     join shs in _db.StudentsHasSubjects on student.StudentId equals shs.StudentId
                                     into studentsHasSubjects
                                     from sshs in studentsHasSubjects.DefaultIfEmpty()
                                     select new
                                     {
                                         Student = new StudentDTO
                                         {
                                             StudentId = student.StudentId,
                                             Firstname = student.Firstname,
                                             Lastname = student.Lastname,
                                             Email = student.Email,
                                             AlternativeEmail = student.AlternativeEmail,
                                             Phonenumber = student.Phonenumber,
                                             Year = student.Year,
                                             Shortpaper = studentShort != null ? new ShortpaperForStudentDTO
                                             {
                                                 ShortpaperId = studentShort.ShortpaperId,
                                                 ShortpaperTopic = studentShort.ShortpaperTopic,
                                             } : null,
                                         },
                                         //Subjects = (from sps in _db.Subjects
                                         //            where sps.SubjectId == studentShort.SubjectId
                                         //            select new SubjectDTO
                                         //            {
                                         //                SubjectId = sps.SubjectId,
                                         //                SubjectName = sps.SubjectName,                                                      
                                         //            }).FirstOrDefault(),
                                         Committee = c != null ? new CommitteeDTO
                                         {
                                             CommitteeId = c.CommitteeId,
                                             Firstname = c.Firstname,
                                             Lastname = c.Lastname,
                                             Email = c.Email,
                                             AlternativeEmail = c.AlternativeEmail,
                                             Phonenumber = c.Phonenumber,
                                             IsAdvisor = shc.IsAdvisor,
                                             IsPrincipal = shc.IsPrincipal,
                                             IsCommittee = shc.IsCommittee,
                                         } : null,
                                         ShortpaperFiles = (from spf in _db.ShortpaperFiles
                                                            join type in _db.ShortpaperFileTypes on spf.ShortpaperFileTypeId equals type.TypeId
                                                            into shortpaperFileDetail
                                                            from spfd in shortpaperFileDetail.DefaultIfEmpty()
                                                            where spf.ShortpaperId == studentShort.ShortpaperId
                                                            select new ShortpaperFileForStudentDTO
                                                            {
                                                                ShortpaperFileId = spf.ShortpaperFileId,
                                                                FileName = spf.FileName,
                                                                Status = spf.Status,
                                                                ShortpaperFileTypeId = spf.ShortpaperFileTypeId
                                                            }).ToList(),
                                         Subjects = (from spf in _db.Subjects
                                                     where spf.SubjectId == sshs.SubjectId
                                                     select new SubjectDTO
                                                     {
                                                         SubjectId = spf.SubjectId,
                                                         SubjectName = spf.SubjectName,
                                                         IsRegisteredSubject = sshs.IsRegisteredSubject,
                                                         IsPaperSubject = sshs.IsPaperSubject
                                                     }).ToList(),
                                     })
                .GroupBy(x => x.Student.StudentId) // Group by StudentId
                .Select(group => new StudentDTO
                {
                    // Select properties from the first item in the group (assuming Student properties are the same for each group)
                    StudentId = group.First().Student.StudentId,
                    Firstname = group.First().Student.Firstname,
                    Lastname = group.First().Student.Lastname,
                    Email = group.First().Student.Email,
                    AlternativeEmail = group.First().Student.AlternativeEmail,
                    Phonenumber = group.First().Student.Phonenumber,
                    Year = group.First().Student.Year,
                    Shortpaper = group.First().Student.Shortpaper,
                    //Subjects = group.First().Subjects,
                    Subjects = group.First().Subjects,
                    Committees = group.Select(x => x.Committee).ToList(),
                    ShortpaperFiles = group.First().ShortpaperFiles
                })
                .ToList();

                }
                else
                {
                         students = (from student in _db.Students
                                     join shortpaper in _db.Shortpapers on student.StudentId equals shortpaper.StudentId
                                     into studentShortpaper
                                     from studentShort in studentShortpaper.DefaultIfEmpty()
                                     join h in _db.ShortpapersHasCommittees on studentShort.ShortpaperId equals h.ShortpaperId
                                     into shortperHasCommittee
                                     from shc in shortperHasCommittee.DefaultIfEmpty()
                                     join committee in _db.Committees on shc.CommitteeId equals committee.CommitteeId
                                     into committees
                                     from c in committees.DefaultIfEmpty()
                                     join shs in _db.StudentsHasSubjects on student.StudentId equals shs.StudentId
                                     into studentsHasSubjects
                                     from sshs in studentsHasSubjects.DefaultIfEmpty()
                                     where (string.IsNullOrEmpty(searchText) ||
                                      student.Firstname.Contains(searchText) ||
                                      student.Lastname.Contains(searchText) ||
                                      student.StudentId.Contains(searchText) ||
                                      student.Email.Contains(searchText))
                                     select new
                                     {
                                         Student = new StudentDTO
                                         {
                                             StudentId = student.StudentId,
                                             Firstname = student.Firstname,
                                             Lastname = student.Lastname,
                                             Email = student.Email,
                                             AlternativeEmail = student.AlternativeEmail,
                                             Phonenumber = student.Phonenumber,
                                             Year = student.Year,
                                             Shortpaper = studentShort != null ? new ShortpaperForStudentDTO
                                             {
                                                 ShortpaperId = studentShort.ShortpaperId,
                                                 ShortpaperTopic = studentShort.ShortpaperTopic,
                                             } : null,
                                         },
                                         //Subjects = (from sps in _db.Subjects
                                         //            where sps.SubjectId == studentShort.SubjectId
                                         //            select new SubjectDTO
                                         //            {
                                         //                SubjectId = sps.SubjectId,
                                         //                SubjectName = sps.SubjectName,
                                         //            }).FirstOrDefault(),
                                         Committee = c != null ? new CommitteeDTO
                                         {
                                             CommitteeId = c.CommitteeId,
                                             Firstname = c.Firstname,
                                             Lastname = c.Lastname,
                                             Email = c.Email,
                                             AlternativeEmail = c.AlternativeEmail,
                                             Phonenumber = c.Phonenumber,
                                             IsAdvisor = shc.IsAdvisor,
                                             IsPrincipal = shc.IsPrincipal,
                                             IsCommittee = shc.IsCommittee,
                                         } : null,
                                         ShortpaperFiles = (from spf in _db.ShortpaperFiles
                                                            join type in _db.ShortpaperFileTypes on spf.ShortpaperFileTypeId equals type.TypeId
                                                            into shortpaperFileDetail
                                                            from spfd in shortpaperFileDetail.DefaultIfEmpty()
                                                            where spf.ShortpaperId == studentShort.ShortpaperId
                                                            select new ShortpaperFileForStudentDTO
                                                            {
                                                                ShortpaperFileId = spf.ShortpaperFileId,
                                                                FileName = spf.FileName,
                                                                Status = spf.Status,
                                                                ShortpaperFileTypeId = spf.ShortpaperFileTypeId
                                                            }).ToList(),
                                         Subjects = (from spf in _db.Subjects
                                                     where spf.SubjectId == sshs.SubjectId
                                                     select new SubjectDTO
                                                     {
                                                         SubjectId = spf.SubjectId,
                                                         SubjectName = spf.SubjectName,
                                                         IsRegisteredSubject = sshs.IsRegisteredSubject,
                                                         IsPaperSubject = sshs.IsPaperSubject
                                                     }).ToList(),
                                     })
                .GroupBy(x => x.Student.StudentId) // Group by StudentId
                .Select(group => new StudentDTO
                {
                    // Select properties from the first item in the group (assuming Student properties are the same for each group)
                    StudentId = group.First().Student.StudentId,
                    Firstname = group.First().Student.Firstname,
                    Lastname = group.First().Student.Lastname,
                    Email = group.First().Student.Email,
                    AlternativeEmail = group.First().Student.AlternativeEmail,
                    Phonenumber = group.First().Student.Phonenumber,
                    Year = group.First().Student.Year,
                    Shortpaper = group.First().Student.Shortpaper,
                    //Subjects = group.First().Subjects,
                    Subjects = group.First().Subjects,
                    Committees = group.Select(x => x.Committee).ToList(),
                    ShortpaperFiles = group.First().ShortpaperFiles
                })
                .ToList();

                }
                if (students.Count == 0)
                {
                    var noresult = new ServiceResponse<List<StudentDTO>>()
                    {
                        httpStatusCode = StatusCodes.Status404NotFound,
                        ErrorMessage = "no students found."
                    };
                    return noresult;
                }
                else
                {
                    var result = new ServiceResponse<List<StudentDTO>>()
                    {
                        httpStatusCode = StatusCodes.Status200OK,
                        Data = students
                    };

                    return result;
                }
            }
            catch (Exception ex)
            {

                var result = new ServiceResponse<List<StudentDTO>>()
                {
                    httpStatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = ex.Message
                };

                return result;
            }
        }

        public ServiceResponse<StudentDTO> GetStudent(string id)
        {
            try {
                var student = (from s in _db.Students
                               join shortpaper in _db.Shortpapers on s.StudentId equals shortpaper.StudentId
                               into studentShortpaper
                               from studentShort in studentShortpaper.DefaultIfEmpty()
                               join h in _db.ShortpapersHasCommittees on studentShort.ShortpaperId equals h.ShortpaperId
                               into shortperHasCommittee
                               from shc in shortperHasCommittee.DefaultIfEmpty()
                               join committee in _db.Committees on shc.CommitteeId equals committee.CommitteeId
                               into committees
                               from c in committees.DefaultIfEmpty()
                               join shs in _db.StudentsHasSubjects on s.StudentId equals shs.StudentId
                               into studentsHasSubjects
                               from sshs in studentsHasSubjects.DefaultIfEmpty()
                               where s.StudentId == id
                               select new
                               {
                                   Student = new StudentDTO
                                   {
                                       StudentId = s.StudentId,
                                       Firstname = s.Firstname,
                                       Lastname = s.Lastname,
                                       Email = s.Email,
                                       AlternativeEmail = s.AlternativeEmail,
                                       Phonenumber = s.Phonenumber,
                                       Year = s.Year,
                                       Shortpaper = studentShort != null ? new ShortpaperForStudentDTO
                                       {
                                           ShortpaperId = studentShort.ShortpaperId,
                                           ShortpaperTopic = studentShort.ShortpaperTopic,
                                       } : null,
                                   },
                                   //Subjects = (from sps in _db.Subjects
                                   //            where sps.SubjectId == studentShort.SubjectId
                                   //            select new SubjectDTO
                                   //            {
                                   //                SubjectId = sps.SubjectId,
                                   //                SubjectName = sps.SubjectName,
                                   //            }).FirstOrDefault(),
                                   Committee = c != null ? new CommitteeDTO
                                   {
                                       CommitteeId = c.CommitteeId,
                                       Firstname = c.Firstname,
                                       Lastname = c.Lastname,
                                       Email = c.Email,
                                       AlternativeEmail = c.AlternativeEmail,
                                       Phonenumber = c.Phonenumber,
                                       IsAdvisor = shc.IsAdvisor,
                                       IsPrincipal = shc.IsPrincipal,
                                       IsCommittee = shc.IsCommittee,
                                   } : null,
                                   ShortpaperFiles = (from spf in _db.ShortpaperFiles
                                                      join type in _db.ShortpaperFileTypes on spf.ShortpaperFileTypeId equals type.TypeId
                                                      into shortpaperFileDetail
                                                      from spfd in shortpaperFileDetail.DefaultIfEmpty()
                                                      where spf.ShortpaperId == studentShort.ShortpaperId
                                                      select new ShortpaperFileForStudentDTO
                                                      {
                                                          ShortpaperFileId = spf.ShortpaperFileId,
                                                          FileName = spf.FileName,
                                                          Status = spf.Status,
                                                          ShortpaperFileTypeId = spf.ShortpaperFileTypeId
                                                      }).ToList(),
                                   Subjects = (from spf in _db.Subjects
                                               where spf.SubjectId == sshs.SubjectId
                                               select new SubjectDTO
                                               {
                                                   SubjectId = spf.SubjectId,
                                                   SubjectName = spf.SubjectName,
                                                   IsRegisteredSubject = sshs.IsRegisteredSubject,
                                                   IsPaperSubject = sshs.IsPaperSubject
                                               }).ToList(),
                               })
                .GroupBy(x => x.Student.StudentId) // Group by StudentId
                .Select(group => new StudentDTO
                {
                    // Select properties from the first item in the group (assuming Student properties are the same for each group)
                    StudentId = group.First().Student.StudentId,
                    Firstname = group.First().Student.Firstname,
                    Lastname = group.First().Student.Lastname,
                    Email = group.First().Student.Email,
                    AlternativeEmail = group.First().Student.AlternativeEmail,
                    Phonenumber = group.First().Student.Phonenumber,
                    Year = group.First().Student.Year,
                    Shortpaper = group.First().Student.Shortpaper,
                    //Subjects = group.First().Subjects,
                    Subjects = group.First().Subjects,
                    Committees = group.Select(x => x.Committee).ToList(),
                    ShortpaperFiles = group.First().ShortpaperFiles
                })
                .FirstOrDefault();


                var result = new ServiceResponse<StudentDTO>()
                {
                    httpStatusCode = StatusCodes.Status200OK,
                    Data = student
                };

                return result;
            }
            catch (Exception ex)
            {

                var result = new ServiceResponse<StudentDTO>()
                {
                    httpStatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = ex.Message
                };

                return result;
            }
        }

        public ServiceResponse<CreateStudentDTO> CreateStudent(CreateStudentDTO newStudent)
        {
            var response = new ServiceResponse<CreateStudentDTO>();

            try
            {
                if (string.IsNullOrEmpty(newStudent.Firstname) || string.IsNullOrEmpty(newStudent.Lastname) || string.IsNullOrEmpty(newStudent.Email) || string.IsNullOrEmpty(newStudent.Phonenumber) || string.IsNullOrEmpty(newStudent.Password))
                {
                    response.ErrorMessage = "Firstname, Lastname, Email, Password, and PhoneNumber are required.";
                    response.httpStatusCode = StatusCodes.Status400BadRequest;
                    return response;
                }

                if (!IsValidEmail(newStudent.Email))
                {
                    response.ErrorMessage = "Invalid email format";
                    response.httpStatusCode = StatusCodes.Status400BadRequest;
                    return response;
                }

                if (newStudent.Phonenumber.Length > 10)
                {
                    response.ErrorMessage = "Invalid phone number";
                    response.httpStatusCode = StatusCodes.Status400BadRequest;
                    return response;
                }

                // Encode the password
                var encodedPassword = EncodePassword(newStudent.Email, newStudent.Password);

                var userEntity = new Student
                {
                    StudentId = newStudent.StudentId,
                    Firstname = newStudent.Firstname,
                    Lastname = newStudent.Lastname,
                    Email = newStudent.Email,
                    AlternativeEmail = newStudent.AlternativeEmail,
                    Phonenumber = newStudent.Phonenumber,
                    Year = newStudent.Year,
                    Password = encodedPassword // Store the encoded password
                };

                _db.Students.Add(userEntity);

                _db.SaveChanges();

                response.IsSuccess = true;
                response.Data = newStudent;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "An unexpected error occurred";
                response.httpStatusCode = StatusCodes.Status500InternalServerError;
            }

            return response;
        }

        public static string EncodePassword(string email, string password)
        {
            // Combine email and password
            string combinedString = $"{email}:{password}";

            // Encode the combined string using Base64
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(combinedString));
        }

        public async Task<ServiceResponse<List<CreateStudentDTO>>> AddStudentsFromCsvAsync(IFormFile csvFile)
        {
            var response = new ServiceResponse<List<CreateStudentDTO>>();

            try
            {
                using (var reader = new StreamReader(csvFile.OpenReadStream()))
                {
                    var studentsToAdd = new List<Student>();
                    bool isFirstRow = true;

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (isFirstRow)
                        {
                            isFirstRow = false;
                            continue; // Skip the first row
                        }

                        var values = line.Split(',');

                        if (values.Length >= 6) // Assuming CSV format: StudentId,Firstname,Lastname,Email,Password,Phonenumber,Year
                        {
                            var studentId = values[0].Trim();
                            var existingStudent = await _db.Students.FirstOrDefaultAsync(s => s.StudentId == studentId);

                            if (existingStudent != null)
                            {
                                // Update existing student's status to "old"
                                existingStudent.Status = "old";
                                
                            }
                            else
                            {
                                // Create a new student
                                var student = new Student
                                {
                                    StudentId = values[0].Trim(),
                                    Firstname = values[1].Trim(),
                                    Lastname = values[2].Trim(),
                                    Email = values[3].Trim(),
                                    Password = values[4].Trim(),
                                    Phonenumber = values[5].Trim(),
                                    Year = values[6].Trim()
                                };

                                studentsToAdd.Add(student);
                            }
                        }
                    }

                    // Add new students to the database
                    _db.Students.AddRange(studentsToAdd);
                    await _db.SaveChangesAsync();

                    // Convert added students to DTOs for response
                    var addedStudents = studentsToAdd.Select(s => new CreateStudentDTO
                    {
                        StudentId = s.StudentId,
                        Firstname = s.Firstname,
                        Lastname = s.Lastname,
                        Email = s.Email,
                        Password = s.Password,
                        Phonenumber = s.Phonenumber,
                        Year = s.Year
                    }).ToList();

                    response.IsSuccess = true;
                    response.Data = addedStudents;
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "An unexpected error occurred while adding students from CSV.";
                response.httpStatusCode = StatusCodes.Status500InternalServerError;
            }

            return response;
        }


        [ProducesResponseType(typeof(ServiceResponse<UpdateStudentDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResponse<UpdateStudentDTO>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServiceResponse<UpdateStudentDTO>), StatusCodes.Status500InternalServerError)]
        public ServiceResponse<UpdateStudentDTO> UpdateStudent(UpdateStudentDTO student)
        {
            var response = new ServiceResponse<UpdateStudentDTO>();

            try
            {
                if (string.IsNullOrEmpty(student.Firstname) || string.IsNullOrEmpty(student.Lastname) || string.IsNullOrEmpty(student.Email) || string.IsNullOrEmpty(student.Phonenumber))
                {
                    response.ErrorMessage = "Firstname, Lastname, Email, and PhoneNumber are required.";
                    response.httpStatusCode = StatusCodes.Status400BadRequest;
                    return response;
                }

                if (!IsValidEmail(student.Email))
                {
                    response.ErrorMessage = "Invalid email format";
                    response.httpStatusCode = StatusCodes.Status400BadRequest;
                    return response;
                }

                if (student.Phonenumber.Length > 10)
                {
                    response.ErrorMessage = "Invalid phone number";
                    response.httpStatusCode = StatusCodes.Status400BadRequest;
                    return response;
                }

                var updateStudent = _db.Students.Include(s => s.StudentsHasSubjects)
                                        .SingleOrDefault(a => a.StudentId == student.StudentId);

                if (updateStudent == null)
                {
                    response.ErrorMessage = "Student not found";
                    response.httpStatusCode = StatusCodes.Status404NotFound;
                    return response;
                }

                updateStudent.Firstname = student.Firstname;
                updateStudent.Lastname = student.Lastname;
                updateStudent.Email = student.Email;
                updateStudent.AlternativeEmail = student.AlternativeEmail;
                updateStudent.Phonenumber = student.Phonenumber;

                // Clear existing subjects
                updateStudent.StudentsHasSubjects.Clear();

                // Add new subjects with additional properties
                if (student.Subjects != null && student.Subjects.Any())
                {
                    foreach (var subject in student.Subjects)
                    {
                        var studentHasSubject = new StudentsHasSubject
                        {
                            StudentId = student.StudentId,
                            SubjectId = subject.SubjectId,
                            IsRegisteredSubject = subject.IsRegisteredSubject ? 1UL : 0UL,
                            IsPaperSubject = subject.IsPaperSubject ? 1UL : 0UL
                        };
                        updateStudent.StudentsHasSubjects.Add(studentHasSubject);
                    }
                }

                _db.SaveChanges();

                response.IsSuccess = true;
                response.Data = student;
            }
            catch (Exception ex)
            {

                response.ErrorMessage = "An unexpected error occurred";
                response.httpStatusCode = StatusCodes.Status500InternalServerError;
            }

            return response;
        }


        private bool IsValidEmail(string email)
        {
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

        //       public UpdateStudentDTO UpdateUserForAdmin(UpdateStudentDTO user)
        //       {
        //       var updateUser = (from a in _db.Students
        //       where a.StudentId == user.StudentId
        //       select a).FirstOrDefault();
        //
        //       updateUser.StudentId = user.StudentId;
        //       updateUser.Firstname = user.Firstname;
        //       updateUser.Lastname = user.Lastname;
        //       updateUser.Email = user.Email;
        //       updateUser.AlternativeEmail = user.AlternativeEmail;
        //       updateUser.Phonenumber = user.Phonenumber;
        //
        //       _db.SaveChanges();
        //
        //       return user;
        //       }

        public Student DeleteStudent(string id)
        {
            var deleteUser = (from a in _db.Students
                              where a.StudentId == id
                              select a).FirstOrDefault();

            _db.Students.Remove(deleteUser);

            _db.SaveChanges();

            return deleteUser;
        }

        public ServiceResponse<List<string>> GetUniqueYears()
        {
            try
            {
                var years = _db.Students.Select(student => student.Year).Distinct().ToList();

                var result = new ServiceResponse<List<string>>()
                {
                    httpStatusCode = StatusCodes.Status200OK,
                    Data = years
                };

                return result;
            }
            catch (Exception ex)
            {
                var result = new ServiceResponse<List<string>>()
                {
                    httpStatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = ex.Message
                };

                return result;
            }
        }
    }
}
