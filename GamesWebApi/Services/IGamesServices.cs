﻿using GamesWebApi.Models;
using GamesWebApi.ViewModels;

namespace GamesWebApi.Services
{
    public interface IGamesServices
    {
         IEnumerable<Game> GetAll();
         Game? GetById(int id);
         Task Create(CreateGameFormViewModel model);
         Task<Game?> Update(EditGameFormViewModel model);
         bool Delete(int id);
    }
}