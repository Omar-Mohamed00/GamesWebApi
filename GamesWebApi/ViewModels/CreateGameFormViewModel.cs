using GamesWebApi.Attributes;
using GamesWebApi.Settings;

namespace GamesWebApi.ViewModels
{
	public class CreateGameFormViewModel: GameFormViewModel
	{
		[AllowedExtentions(FileSettings.AllowedExtention),
			MaxFileSize(FileSettings.MaxFileSizeInBytes)]
		public IFormFile Cover { get; set; } = default!;
    }
}