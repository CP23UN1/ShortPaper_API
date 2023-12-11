using Microsoft.EntityFrameworkCore;
using ShortPaper_API.DTO;
using ShortPaper_API.Entities;

namespace ShortPaper_API.Services.Projects
{
    public class ProjectService : IProjectService
    {
        private readonly ShortpaperDbContext _db;
        public ProjectService(ShortpaperDbContext db)
        {
            _db = db;
        }

        public List<ProjectDTO> GetProjects()
        {
            var projects = (from a in _db.Projects
                            join b in _db.Users on a.StudentId equals b.UserId
                            into userStudent
                            from student in userStudent.DefaultIfEmpty()
                            join c in _db.Users on a.CommitteeFirst equals c.UserId
                            into userCommitFirst
                            from committeeFirst in userCommitFirst.DefaultIfEmpty()
                            join d in _db.Users on a.CommitteeSecond equals d.UserId
                            into userCommitSecond
                            from committeeSecond in userCommitSecond.DefaultIfEmpty()
                            join e in _db.Users on a.CommitteeThird equals e.UserId
                            into userCommitThird
                            from committeeThird in userCommitThird.DefaultIfEmpty()
                            select new ProjectDTO
                            {
                                ProjectId = a.ProjectId,
                                Topic = a.Topic,
                                Student = new ProjectInfoDTO
                                {
                                    UserId = student.UserId,
                                    StudentId = student.StudentId,
                                    Firstname = student.Firstname,
                                    Lastname = student.Lastname
                                },
                                CommitteeFirst = committeeFirst != null ? new ProjectInfoDTO
                                {
                                    UserId = committeeFirst.UserId,
                                    Firstname = committeeFirst.Firstname,
                                    Lastname = committeeFirst.Lastname,
                                } : null,
                                CommitteeSecond = committeeSecond != null ? new ProjectInfoDTO
                                {
                                    UserId = committeeSecond.UserId,
                                    Firstname = committeeSecond.Firstname,
                                    Lastname = committeeSecond.Lastname,
                                } : null,
                                CommitteeThird = committeeThird != null ? new ProjectInfoDTO
                                {
                                    UserId = committeeThird.UserId,
                                    Firstname = committeeThird.Firstname,
                                    Lastname = committeeThird.Lastname,
                                } : null
                            }).ToList();

            return projects;
        }

        public ProjectDTO GetProject(int id)
        {
            var project = (from a in _db.Projects
                            join b in _db.Users on a.StudentId equals b.UserId
                            into userStudent
                            from student in userStudent.DefaultIfEmpty()
                            join c in _db.Users on a.CommitteeFirst equals c.UserId
                            into userCommitFirst
                            from committeeFirst in userCommitFirst.DefaultIfEmpty()
                            join d in _db.Users on a.CommitteeSecond equals d.UserId
                            into userCommitSecond
                            from committeeSecond in userCommitSecond.DefaultIfEmpty()
                            join e in _db.Users on a.CommitteeThird equals e.UserId
                            into userCommitThird
                            from committeeThird in userCommitThird.DefaultIfEmpty()
                            where a.StudentId == id
                            select new ProjectDTO
                            {
                                ProjectId = a.ProjectId,
                                Topic = a.Topic,
                                Student = new ProjectInfoDTO
                                {
                                    UserId = student.UserId,
                                    StudentId = student.StudentId,
                                    Firstname = student.Firstname,
                                    Lastname = student.Lastname
                                },
                                CommitteeFirst = committeeFirst != null ? new ProjectInfoDTO
                                {
                                    UserId = committeeFirst.UserId,
                                    Firstname = committeeFirst.Firstname,
                                    Lastname = committeeFirst.Lastname,
                                } : null,
                                CommitteeSecond = committeeSecond != null ? new ProjectInfoDTO
                                {
                                    UserId = committeeSecond.UserId,
                                    Firstname = committeeSecond.Firstname,
                                    Lastname = committeeSecond.Lastname,
                                } : null,
                                CommitteeThird = committeeThird != null ? new ProjectInfoDTO
                                {
                                    UserId = committeeThird.UserId,
                                    Firstname = committeeThird.Firstname,
                                    Lastname = committeeThird.Lastname,
                                } : null
                            }).FirstOrDefault();

            return project;
        }

        public void ChooseCommitteeMembers(int projectId, int advisorId, int advisorId2, int advisorId3)
        {
            var project = _db.Projects.SingleOrDefault(p => p.ProjectId == projectId);

            if (project == null)
            {
                return;
            }
                    project.CommitteeFirst = advisorId;
                    project.CommitteeSecond = advisorId2;
                    project.CommitteeThird = advisorId3;

                _db.SaveChanges();
        }
    }
}
