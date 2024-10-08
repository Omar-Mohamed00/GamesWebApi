using GamesWebApi.Models;
using GamesWebApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GamesWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IGamesServices _gamesServices;

        public HomeController(IGamesServices gamesServices)
        {
            _gamesServices = gamesServices;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var games = _gamesServices.GetAll();
            return Ok(games);
        }
    }
}