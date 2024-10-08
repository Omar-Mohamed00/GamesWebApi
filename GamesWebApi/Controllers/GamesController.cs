using GamesWebApi.Services;
using GamesWebApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GamesWebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly ICategoriesServices _categoriesServices;
        private readonly IDevicesServices _deviceServices;
        private readonly IGamesServices _gamesServices;

        public GamesController(ICategoriesServices categoriesServices
            , IDevicesServices deviceServices
            , IGamesServices gamesServices)
        {
            _categoriesServices = categoriesServices;
            _deviceServices = deviceServices;
            _gamesServices = gamesServices;
        }
        [HttpGet]
        public  IActionResult Get()
        {
            var games = _gamesServices.GetAll().ToList();
            if (games is null)
                return NotFound();
            return Ok(games);
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var game = _gamesServices.GetById(id);
            if (game is null)
                return NotFound();
            return Ok(game);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateGameFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = _categoriesServices.GetSelectList().Cast<SelectListItem>().ToList();
                model.Devices = _deviceServices.GetSelectList().Cast<SelectListItem>().ToList();
                return NotFound(model);
            }

            await _gamesServices.Create(model);

            return Ok(model);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(EditGameFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = _categoriesServices.GetSelectList().Cast<SelectListItem>().ToList();
                model.Devices = _deviceServices.GetSelectList().Cast<SelectListItem>().ToList();
                return BadRequest();
            }

            var game = await _gamesServices.Update(model);
            if (game is null)
                return NotFound();

            return Ok(game);
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var isDeleted = _gamesServices.Delete(id);

            return isDeleted ? Ok() : BadRequest();
        }
    }
}
