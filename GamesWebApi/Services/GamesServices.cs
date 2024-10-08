using GamesWebApi.ViewModels;
using GamesWebApi.Data;
using GamesWebApi.Models;
using GamesWebApi.Settings;
using Microsoft.EntityFrameworkCore;

namespace GamesWebApi.Services
{
    public class GamesServices:IGamesServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _imagesPath;

        public GamesServices(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _imagesPath = $"{_webHostEnvironment.WebRootPath}{FileSettings.ImagesPath}";
        }
        public IEnumerable<Game> GetAll()
        {
            return _context.Games
                .Include(g=> g.Category)
                .Include(g=> g.Devices)
                .ThenInclude(d=> d.Device)
                .AsNoTracking().ToList();
        }
        public Game? GetById(int id)
        {
            return _context.Games
               .Include(g => g.Category)
               .Include(g => g.Devices)
               .ThenInclude(d => d.Device)
               .AsNoTracking().SingleOrDefault(g => g.Id == id);
        }

        public async Task Create(CreateGameFormViewModel model)
        {
            var coverName = await SaveCover(model.Cover);
            Game game = new()
            {
                Name = model.Name,
                Description = model.Description,
                CategoryId = model.CategoryId,
                Cover = coverName,
                Devices = model.SelectDevices.Select(d => new GameDevice { DeviceId = d }).ToList()
            };
            _context.Add(game);
            _context.SaveChanges();
        }

        public async Task<Game?> Update(EditGameFormViewModel model)
        {
            var game = _context.Games
                 .Include(g => g.Devices)
                 .SingleOrDefault(g => g.Id == model.Id);

            if(game is null)
                return null;

            var hasNewCover = model.Cover is not null;
            var oldCover = game.Cover;

            game.Name = model.Name;
            game.Description = model.Description;
            game.CategoryId = model.CategoryId;
            game.Devices = model.SelectDevices.Select(d => new GameDevice { DeviceId= d }).ToList();    

            if(hasNewCover )
            {
                game.Cover = await SaveCover(model.Cover!);
            }
            var effectedRows = _context.SaveChanges();

            if(effectedRows > 0 )
            {
                if (hasNewCover)
                {
                    var cover = Path.Combine(_imagesPath, oldCover);
                    File.Delete(cover);
                };
                return game;
            }
            else
            {
                var cover = Path.Combine(_imagesPath, game.Cover);
                File.Delete(cover);
                return null;
            }

        }

        public bool Delete(int id)
        {
            var isDelated = false;

            var game = _context.Games.Find(id);

            if (game is null) 
                return isDelated;

            _context.Remove(game);

            var effectedRows = _context.SaveChanges();

            if(effectedRows > 0 )
            {
                isDelated = true;
                var cover = Path.Combine(_imagesPath, game.Cover);
                File.Delete(cover);
            }
            return isDelated;
        }

        private async Task<string> SaveCover(IFormFile cover)
        {
            var coverName = $"{Guid.NewGuid()}{Path.GetExtension(cover.FileName)}";
            var path = Path.Combine(_imagesPath, coverName);
            using var stream = File.Create(path);
            await cover.CopyToAsync(stream);
            return coverName;
        }
    }
}