namespace Mcpgnz.DesktopFramework
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;

    [Serializable]
    public static class DesktopEx
    {
        #region Public Types
        public enum FolderFlags : ulong
        {
            FWF_NONE = 0,
            FWF_AUTOARRANGE = 0x1,
            FWF_ABBREVIATEDNAMES = 0x2,
            FWF_SNAPTOGRID = 0x4,
            FWF_OWNERDATA = 0x8,
            FWF_BESTFITWINDOW = 0x10,
            FWF_DESKTOP = 0x20,
            FWF_SINGLESEL = 0x40,
            FWF_NOSUBFOLDERS = 0x80,
            FWF_TRANSPARENT = 0x100,
            FWF_NOCLIENTEDGE = 0x200,
            FWF_NOSCROLL = 0x400,
            FWF_ALIGNLEFT = 0x800,
            FWF_NOICONS = 0x1000,
            FWF_SHOWSELALWAYS = 0x2000,
            FWF_NOVISIBLE = 0x4000,
            FWF_SINGLECLICKACTIVATE = 0x8000,
            FWF_NOWEBVIEW = 0x10000,
            FWF_HIDEFILENAMES = 0x20000,
            FWF_CHECKSELECT = 0x40000,
            FWF_NOENUMREFRESH = 0x80000,
            FWF_NOGROUPING = 0x100000,
            FWF_FULLROWSELECT = 0x200000,
            FWF_NOFILTERS = 0x400000,
            FWF_NOCOLUMNHEADER = 0x800000,
            FWF_NOHEADERINALLVIEWS = 0x1000000,
            FWF_EXTENDEDTILES = 0x2000000,
            FWF_TRICHECKSELECT = 0x4000000,
            FWF_AUTOCHECKSELECT = 0x8000000,
            FWF_NOBROWSERVIEWSTATE = 0x10000000,
            FWF_SUBSETGROUPS = 0x20000000,
            FWF_USESEARCHFOLDER = 0x40000000,
            FWF_ALLOWRTLREADING = 0x80000000
        }
        #endregion Public Types

        #region Public Variables
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

        // note: rename is reported as delete + create
        public static Action<string> OnItemCreated;
        public static Action<string> OnItemDeleted;
        public static Action<string> OnItemChanged;
        #endregion Public Variables

        #region Public Methods
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

        public static bool Style(FolderFlags flag)
        {
            return desktop_get_style(flag);
        }
        public static void Style(FolderFlags flag, bool state)
        {
            desktop_set_style(flag, state);
        }
        #endregion Public Methods

        #region Private Variables
        private static string _AbsolutePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private static FileSystemWatcher _Watcher = null;
        #endregion Private Variables

        #region Private Methods
        static DesktopEx()
        {
            _Watcher = new FileSystemWatcher(_AbsolutePath);
            _Watcher.Created += (object sender, FileSystemEventArgs args) => OnItemCreated?.Invoke(args.Name);
            _Watcher.Deleted += (object sender, FileSystemEventArgs args) => OnItemDeleted?.Invoke(args.Name);
            _Watcher.Changed += (object sender, FileSystemEventArgs args) => OnItemChanged?.Invoke(args.Name);
            _Watcher.EnableRaisingEvents = true;
        }
        #endregion Private Methods

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
        internal static extern bool desktop_get_style(FolderFlags flag);

        [DllImport("desktop-lib.dll")]
        internal static extern void desktop_set_style(FolderFlags flag, bool state);
        #endregion Import
    }
}