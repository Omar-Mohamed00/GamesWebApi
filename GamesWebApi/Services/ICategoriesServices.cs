using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesWebApi.Services
{
    public interface ICategoriesServices
    {
        IEnumerable<SelectListItem> GetSelectList();
    }
}