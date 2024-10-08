using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesWebApi.Services
{
    public interface IDevicesServices
    {

        IEnumerable<SelectListItem> GetSelectList();
    }
}
