using ShortPaper_API.Entities;
namespace ShortPaper_API.Services.Users
{
    public interface IUserService
    {
        List<User> GetUsers();

        User GetUser(int id);

    }
}
