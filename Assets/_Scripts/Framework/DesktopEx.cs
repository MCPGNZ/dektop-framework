namespace Mcpgnz.DesktopFramework
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;
    using Sirenix.OdinInspector;

    [Serializable]
    public static class DesktopEx
    {
        #region Public Variables
        [ShowInInspector]
        public static bool AutoArrange
        {
            get => desktop_get_autoarrange();
            set => desktop_set_autoarrange(value);
        }

        [ShowInInspector]
        public static bool GridAlign
        {
            get => desktop_get_gridallign();
            set => desktop_set_gridallign(value);
        }

        public static List<FileEx> Files
        {
            get
            {
                var result = new List<FileEx>();
                var files = Directory.GetFiles(_AbsolutePath);
                foreach (var file in files)
                {
                    var info = new FileInfo(file);
                    result.Add(new FileEx(info));
                }

                return result;
            }
        }
        public static List<DirectoryEx> Directories
        {
            get
            {
                var result = new List<DirectoryEx>();
                var dirs = Directory.GetDirectories(_AbsolutePath);
                foreach (var dir in dirs)
                {
                    var info = new DirectoryInfo(dir);
                    result.Add(new DirectoryEx(info));
                }

                return result;
            }
        }
        #endregion Public Variables

        #region Public methods
        public static FileEx CreateFile(string name)
        {
            var fileInfo = new FileInfo(Path.Combine(_AbsolutePath, name));
            using (var stream = fileInfo.Create()) { }

            return new FileEx(fileInfo);
        }
        public static DirectoryEx CreateDirectory(string name)
        {
            var path = Path.Combine(_AbsolutePath, name);
            var directory = Directory.CreateDirectory(path);
            return new DirectoryEx(directory);
        }
        #endregion Public methods

        #region Private Variables
        private static string _AbsolutePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        #endregion Private Variables

        #region Import
        [DllImport("desktop-lib.dll")]
        internal static extern void desktop_initialize();

        [DllImport("desktop-lib.dll")]
        internal static extern void desktop_get_item_position(
            [MarshalAs(UnmanagedType.LPWStr)] string path, out int x, out int y);

        [DllImport("desktop-lib.dll")]
        internal static extern void desktop_set_item_position(
            [MarshalAs(UnmanagedType.LPWStr)] string path, int x, int y);

        [DllImport("desktop-lib.dll")]
        internal static extern bool desktop_get_autoarrange();

        [DllImport("desktop-lib.dll")]
        internal static extern void desktop_set_autoarrange(bool state);

        [DllImport("desktop-lib.dll")]
        internal static extern bool desktop_get_gridallign();

        [DllImport("desktop-lib.dll")]
        internal static extern void desktop_set_gridallign(bool state);
        #endregion Import
    }
}