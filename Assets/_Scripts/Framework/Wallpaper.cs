namespace Mcpgnz.DesktopFramework.Framework
{
    using Microsoft.Win32;
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using UnityEngine;

    // https://code.4noobz.net/set-the-wallpaper-by-code/
    public static class Wallpaper
    {
        const int SPI_SETDESKWALLPAPER = 20;
        const int SPI_GETDESKWALLPAPER = 0x73;
        const int SPIF_UPDATEINIFILE = 0x01;
        const int SPIF_SENDWININICHANGE = 0x02;
        private static class NativeSet
        {
            [DllImport("user32.dll", EntryPoint = "SystemParametersInfo", CharSet = CharSet.Auto)]
            public static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
        }

        private static class NativeGet
        {

            [DllImport("user32.dll", EntryPoint = "SystemParametersInfo", CharSet = CharSet.Auto)]
            public static extern int SystemParametersInfo(int uAction, int uParam, StringBuilder lpvParam, int fuWinIni);

        }

        public enum Style : int
        {
            Tiled,
            Centered,
            Stretched
        }

        public static void Set(string imgPath, Style? style = null)
        {
            if (imgPath == null)
            {
                throw new ArgumentException("Cowardly refusing to set desktop background to null");
            }
            if (!System.IO.Path.IsPathRooted(imgPath))
            {
                imgPath = UnityEngine.Application.streamingAssetsPath + System.IO.Path.DirectorySeparatorChar + imgPath;
            }

            var key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);

            switch (style)
            {
                case Style.Tiled:
                    key.SetValue(@"WallpaperStyle", 1.ToString());
                    key.SetValue(@"TileWallpaper", 1.ToString());
                    break;
                case Style.Centered:
                    key.SetValue(@"WallpaperStyle", 1.ToString());
                    key.SetValue(@"TileWallpaper", 0.ToString());
                    break;
                case Style.Stretched:
                    key.SetValue(@"WallpaperStyle", 2.ToString());
                    key.SetValue(@"TileWallpaper", 0.ToString());
                    break;
                default:
                    break;
            }

            NativeSet.SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, imgPath, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
            Debug.Log($"Wallpaper set to {imgPath}");
        }

        public static readonly string BackupWallpaperPathFile = UnityEngine.Application.dataPath + System.IO.Path.DirectorySeparatorChar + "backup_wallpaper_path.txt";

        private static string BackupWallpaperPath
        {
            get
            {
                try
                {
                    return File.ReadAllText(BackupWallpaperPathFile);
                }
                catch (FileNotFoundException)
                {
                    return null;
                }
            }
            set
            {
                File.WriteAllText(BackupWallpaperPathFile, value);
            }
        }

        // https://social.msdn.microsoft.com/Forums/en-US/ab83d0c3-0b82-4353-b447-38ad297dfece/how-to-change-the-wallpaper-programmatically?forum=csharpgeneral
        public static void Backup()
        {
            if (BackupWallpaperPath != null)
            {
                Debug.Log($"Wallpaper already backed up ({BackupWallpaperPath}), skipping");
            }
            else
            {
                StringBuilder s = new StringBuilder(300);
                NativeGet.SystemParametersInfo(SPI_GETDESKWALLPAPER, 300, s, 0);
                BackupWallpaperPath = s.ToString();
                Debug.Log($"Wallpaper path {BackupWallpaperPath} backed up to {BackupWallpaperPathFile}");
            }
        }

        public static void Restore()
        {
            Set(BackupWallpaperPath);
            File.Delete(BackupWallpaperPathFile);
        }
    }

}