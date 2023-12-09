﻿using ShortPaper_API.DTO;
using ShortPaper_API.Entities;
using ShortPaper_API.Repositories;

namespace ShortPaper_API.Services.Users
{
    public interface IUserService
    {
        List<UserDTO> GetUsers();
        List<UserDTO> GetStudents();
        UserDTO GetStudent(int id);
        List<UserDTO> GetAdvisors();
        UserDTO GetAdvisor(int id);
        UserDTO GetUser(int id);
        UserDTO CreateUser(UserDTO user);
        UserDTO UpdateUserForStudent(UserDTO user);
        UserDTO UpdateUserForAdmin(UserDTO user);
        User DeleteUser(int id);
    }
}
