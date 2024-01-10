using Microsoft.AspNetCore.Mvc;
using ShortPaper_API.DTO;
using ShortPaper_API.Entities;
using ShortPaper_API.Helper;
using System.Net.Mail;

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
                                join c in _db.ShortpapersHasCommittees on studentShort.ShortpaperId equals c.ShortpaperId
                                into shortperHasCommittee
                                from shc in shortperHasCommittee.DefaultIfEmpty()
                                join d in _db.Committees on shc.CommitteeId equals d.CommitteeId
                                into shortperCommittee
                                from sc in shortperCommittee.DefaultIfEmpty()
                                join e in _db.ShortpaperFiles on studentShort.ShortpaperId equals e.ShortpaperId
                                into shortpaperFile
                                from spf in shortpaperFile.DefaultIfEmpty()
                                join f in _db.Subjects on studentShort.SubjectId equals f.SubjectId
                                into shortpaperSubjects
                                from sps in shortpaperSubjects.DefaultIfEmpty()
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
                                        Subjects = new List<SubjectDTO> { sps != null ? new SubjectDTO
                                        {
                                        SubjectId = sps.SubjectId,
                                        SubjectName = sps.SubjectName,
                                        } : null },
                                        Committees = new List<CommitteeDTO> { sc != null ? new CommitteeDTO
                                        {
                                        CommitteeId = sc.CommitteeId,
                                        Firstname = sc.Firstname,
                                        Lastname = sc.Lastname,
                                        Email = sc.Email,
                                        AlternativeEmail = sc.AlternativeEmail,
                                        Phonenumber = sc.Phonenumber
                                        } : null },
                                    },
                                    ShortpaperFile = spf != null ? new ShortpaperFileDTO
                                    {
                                        ShortpaperFileId = spf.ShortpaperFileId,
                                        FileName = spf.FileName,
                                        Status = spf.Status,
                                    } : null,
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
                     Subjects = group.First().Student.Subjects,
                     Committees = group.First().Student.Committees,
                     // Select all ShortpaperFile entries for the student
                     ShortpaperFiles = group.Select(x => x.ShortpaperFile).ToList(),
                 })
                 .ToList();

                var result = new ServiceResponse<List<StudentDTO>>()
                {
                    httpStatusCode = StatusCodes.Status200OK,
                    Data = students
                };

                return result;
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
                                join c in _db.ShortpapersHasCommittees on studentShort.ShortpaperId equals c.ShortpaperId
                                into shortperHasCommittee
                                from shc in shortperHasCommittee.DefaultIfEmpty()
                                join d in _db.Committees on shc.CommitteeId equals d.CommitteeId
                                into shortperCommittee
                                from sc in shortperCommittee.DefaultIfEmpty()
                                join e in _db.ShortpaperFiles on studentShort.ShortpaperId equals e.ShortpaperId
                                into shortpaperFile
                                from spf in shortpaperFile.DefaultIfEmpty()
                                join f in _db.Subjects on studentShort.SubjectId equals f.SubjectId
                                into shortpaperSubjects
                                from sps in shortpaperSubjects.DefaultIfEmpty()
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
                                        Subjects = new List<SubjectDTO> { sps != null ? new SubjectDTO
                                        {
                                        SubjectId = sps.SubjectId,
                                        SubjectName = sps.SubjectName,
                                        } : null },
                                        Committees = new List<CommitteeDTO> { sc != null ? new CommitteeDTO
                                        {
                                        CommitteeId = sc.CommitteeId,
                                        Firstname = sc.Firstname,
                                        Lastname = sc.Lastname,
                                        Email = sc.Email,
                                        AlternativeEmail = sc.AlternativeEmail,
                                        Phonenumber = sc.Phonenumber
                                        } : null },
                                    },
                                    ShortpaperFile = spf != null ? new ShortpaperFileDTO
                                    {
                                        ShortpaperFileId = spf.ShortpaperFileId,
                                        FileName = spf.FileName,
                                        Status = spf.Status,
                                    } : null,
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
                     Subjects = group.First().Student.Subjects,
                     Committees = group.First().Student.Committees,
                     // Select all ShortpaperFile entries for the student
                     ShortpaperFiles = group.Select(x => x.ShortpaperFile).ToList(),
                 })
                 .ToList();
                }
                else
                {
                    students = (from student in _db.Students
                                join shortpaper in _db.Shortpapers on student.StudentId equals shortpaper.StudentId
                                into studentShortpaper
                                from studentShort in studentShortpaper.DefaultIfEmpty()
                                join c in _db.ShortpapersHasCommittees on studentShort.ShortpaperId equals c.ShortpaperId
                                into shortperHasCommittee
                                from shc in shortperHasCommittee.DefaultIfEmpty()
                                join d in _db.Committees on shc.CommitteeId equals d.CommitteeId
                                into shortperCommittee
                                from sc in shortperCommittee.DefaultIfEmpty()
                                join e in _db.ShortpaperFiles on studentShort.ShortpaperId equals e.ShortpaperId
                                into shortpaperFile
                                from spf in shortpaperFile.DefaultIfEmpty()
                                join f in _db.Subjects on studentShort.SubjectId equals f.SubjectId
                                into shortpaperSubjects
                                from sps in shortpaperSubjects.DefaultIfEmpty()
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
                                        Subjects = new List<SubjectDTO> { sps != null ? new SubjectDTO
                                        {
                                        SubjectId = sps.SubjectId,
                                        SubjectName = sps.SubjectName,
                                        } : null },
                                        Committees = new List<CommitteeDTO> { sc != null ? new CommitteeDTO
                                        {
                                        CommitteeId = sc.CommitteeId,
                                        Firstname = sc.Firstname,
                                        Lastname = sc.Lastname,
                                        Email = sc.Email,
                                        AlternativeEmail = sc.AlternativeEmail,
                                        Phonenumber = sc.Phonenumber
                                        } : null },
                                    },
                                    ShortpaperFile = spf != null ? new ShortpaperFileDTO
                                    {
                                        ShortpaperFileId = spf.ShortpaperFileId,
                                        FileName = spf.FileName,
                                        Status = spf.Status,
                                    } : null,
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
                     Subjects = group.First().Student.Subjects,
                     Committees = group.First().Student.Committees,
                     // Select all ShortpaperFile entries for the student
                     ShortpaperFiles = group.Select(x => x.ShortpaperFile).ToList(),
                 })
                 .ToList();
                }
                    var result = new ServiceResponse<List<StudentDTO>>()
                {
                    httpStatusCode = StatusCodes.Status200OK,
                    Data = students
                };

                return result;
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
                            join c in _db.ShortpapersHasCommittees on studentShort.ShortpaperId equals c.ShortpaperId
                            into shortperHasCommittee
                            from shc in shortperHasCommittee.DefaultIfEmpty()
                            join d in _db.Committees on shc.CommitteeId equals d.CommitteeId
                            into shortperCommittee
                            from sc in shortperCommittee.DefaultIfEmpty()
                            join e in _db.ShortpaperFiles on studentShort.ShortpaperId equals e.ShortpaperId
                            into shortpaperFile
                            from spf in shortpaperFile.DefaultIfEmpty()
                            join f in _db.Subjects on studentShort.SubjectId equals f.SubjectId
                            into shortpaperSubjects
                            from sps in shortpaperSubjects.DefaultIfEmpty()
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
                                    Subjects = new List<SubjectDTO> { sps != null ? new SubjectDTO
                                        {
                                        SubjectId = sps.SubjectId,
                                        SubjectName = sps.SubjectName,
                                        } : null },
                                    Committees = new List<CommitteeDTO> { sc != null ? new CommitteeDTO
                                        {
                                        CommitteeId = sc.CommitteeId,
                                        Firstname = sc.Firstname,
                                        Lastname = sc.Lastname,
                                        Email = sc.Email,
                                        AlternativeEmail = sc.AlternativeEmail,
                                        Phonenumber = sc.Phonenumber
                                        } : null },
                                },
                                ShortpaperFile = spf != null ? new ShortpaperFileDTO
                                {
                                    ShortpaperFileId = spf.ShortpaperFileId,
                                    FileName = spf.FileName,
                                    Status = spf.Status,
                                } : null,
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
                     Subjects = group.First().Student.Subjects,
                     Committees = group.First().Student.Committees,
                     // Select all ShortpaperFile entries for the student
                     ShortpaperFiles = group.Select(x => x.ShortpaperFile).ToList(),
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
                if (string.IsNullOrEmpty(newStudent.Firstname) || string.IsNullOrEmpty(newStudent.Lastname) || string.IsNullOrEmpty(newStudent.Email) || string.IsNullOrEmpty(newStudent.Phonenumber))
                {
                    response.ErrorMessage = "Firstname, Lastname, Email, and PhoneNumber are required.";
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

                var userEntity = new Student
                {
                    StudentId = newStudent.StudentId,
                    Firstname = newStudent.Firstname,
                    Lastname = newStudent.Lastname,
                    Email = newStudent.Email,
                    AlternativeEmail = newStudent.AlternativeEmail,
                    Phonenumber = newStudent.Phonenumber,
                    Year = newStudent.Year
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

                if(!IsValidEmail(student.Email))
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


                var updateStudent = (from a in _db.Students
                                  where a.StudentId == student.StudentId
                                  select a).FirstOrDefault();

                updateStudent.Firstname = student.Firstname;
                updateStudent.Lastname = student.Lastname;
                updateStudent.Email = student.Email;
                updateStudent.AlternativeEmail = student.AlternativeEmail;
                updateStudent.Phonenumber = student.Phonenumber;

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
    }
}
