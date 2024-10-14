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
            try
            {
                var games = _gamesServices.GetAll().ToList();
                if (games is null)
                    return NotFound();
                return Ok(games);
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }
        [HttpGet("{id}")]
        public Task< IActionResult> Get(int id)
        {
            try
            {
                var game = _gamesServices.GetById(id);
                if (game is null)
                    return Task.FromResult<IActionResult>(NotFound());
                return Task.FromResult<IActionResult>(Ok(game));
            }
            catch(Exception ex)
            {
                return Task.FromResult<IActionResult>(BadRequest("An error occurred while processing your request."));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateGameFormViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    model.Categories = _categoriesServices.GetSelectList().Cast<SelectListItem>().ToList();
                    model.Devices = _deviceServices.GetSelectList().Cast<SelectListItem>().ToList();
                    return NotFound(model);
                }

                await _gamesServices.Create(model);

                return Created("",model);
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            } 

            
        }

        [HttpPut]
        public async Task<IActionResult> Edit(EditGameFormViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    model.Categories = _categoriesServices.GetSelectList().Cast<SelectListItem>().ToList();
                    model.Devices = _deviceServices.GetSelectList().Cast<SelectListItem>().ToList();
                    return BadRequest();
                }

                var game = await _gamesServices.Update(model);
                if (game is null)
                    return BadRequest();

                return NoContent();
            }
            catch (Exception ex) 
            { 
                return NotFound(ex.Message);
            }
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var isDeleted = _gamesServices.Delete(id);

            return isDeleted ? Ok() : BadRequest();
        }
    }
}
