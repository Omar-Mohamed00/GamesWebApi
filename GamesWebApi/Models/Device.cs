﻿using System.ComponentModel.DataAnnotations;

namespace GamesWebApi.Models
{
    public class Device: BaseEntity
    {
        [MaxLength(50)]
        public string Icon { get; set; } = string.Empty;
    }
}
