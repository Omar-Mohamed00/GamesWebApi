﻿namespace GamesWebApi.Settings
{
    public static class FileSettings
    {
       public const string ImagesPath = "assets/images/games";
       public const string AllowedExtention = ".jpg,.jpeg,.png";
       public const int MaxFileSizeInMB = 1;
       public const int MaxFileSizeInBytes = MaxFileSizeInMB * 1024 * 1024;
    }
}
