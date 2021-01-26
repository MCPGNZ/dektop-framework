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
        [StructLayout(LayoutKind.Sequential)]
        public struct Item
        {
            [MarshalAs(UnmanagedType.LPWStr)] public string path;
            [MarshalAs(UnmanagedType.SysInt)] public int x;
            [MarshalAs(UnmanagedType.SysInt)] public int y;
        }

        #region Public Variables
        public static List<FileEx> Files
        {
            get
            {
                var result = new List<FileEx>();
                var files = Directory.GetFiles(_Path);
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
                var dirs = Directory.GetDirectories(_Path);
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
        [Button]
        public static DirectoryEx CreateDirectory(string name)
        {
            var path = Path.Combine(_Path, name);
            var directory = Directory.CreateDirectory(path);
            return new DirectoryEx(directory);
        }

        public static void SetItemPosition(string path, int x, int y)
        {
            _Items.Add(new Item { path = path, x = x, y = y });
        }

        public static void Flush()
        {
            desktop_set_item_positions(_Items.ToArray(), _Items.Count);
            _Items.Clear();
        }
        #endregion Public methods

        #region Private Variables
        private static string _Path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private static List<Item> _Items = new List<Item>();
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
        internal static extern void desktop_set_item_positions(
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStruct, SizeParamIndex = 1)] Item[] items,
            long size);
        #endregion Import
    }
}