using ShortPaper_API.Entities;
using ShortPaper_API.Repositories;

namespace ShortPaper_API.Services.Users
{
    public interface IUserService
    {
        List<User> GetUsers();
        User GetUser(int id);
        User CreateUser(User user);
        User UpdateUser(User user);
        int DeleteUser(int id);
    }
}
