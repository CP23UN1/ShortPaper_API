using ShortPaper_API.DTO;
using ShortPaper_API.Helper;

namespace ShortPaper_API.Services.Shortpapers
{
    public interface IShortpaperService
    {
        ServiceResponse<List<ShortpaperDTO>> GetShortpaper();
        ServiceResponse<List<ShortpaperDTO>> GetShortpaperByFilter(string searchText);
    }
}
