namespace Mcpgnz
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [Serializable]
    public static class FrameworkEx
    {
        #region Api
        public static void Initialize()
        {
            Initialize(OnUnmanagedInfo, OnUnmanagedError);
            DesktopEx.desktop_initialize();
        }
        #endregion Api

        #region Import
        [DllImport("desktop-lib.dll", EntryPoint = "framework_initialize")]
        private static extern void Initialize(
            [MarshalAs(UnmanagedType.FunctionPtr)] Callbacks.Info callbackInfo,
            [MarshalAs(UnmanagedType.FunctionPtr)] Callbacks.Error callbackError);

        [DllImport("desktop-lib.dll", EntryPoint = "framework_cleanup")]
        public static extern void Cleanup();
        #endregion Import

        #region Private
        private static void OnUnmanagedInfo(string msg)
        {
            /* todo: add settings */
            // Debug.Log($"<color=#DAF7A6>Success: {msg}</color>");
        }
        private static void OnUnmanagedError(string msg)
        {
            Debug.LogError($"Error: {msg}");
        }
        #endregion Private
    }

    [Serializable]
    public static class DesktopEx
    {
        #region Public Methods
        public static List<FileInfo> Files
        {
            get
            {
                var result = new List<FileInfo>();
                var files = Directory.GetFiles(_Path);
                foreach (var file in files)
                {
                    result.Add(new FileInfo(file));
                }

                return result;
            }
        }
        public static List<DirectoryInfo> Directories
        {
            get
            {
                var result = new List<DirectoryInfo>();
                var dirs = Directory.GetDirectories(_Path);
                foreach (var dir in dirs)
                {
                    result.Add(new DirectoryInfo(dir));
                }

                return result;
            }
        }
        #endregion Public Methods

        #region Private Variables
        private static string _Path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        #endregion Private Variables

        #region Import
        [DllImport("desktop-lib.dll")]
        internal static extern void desktop_initialize();

        [DllImport("desktop-lib.dll")]
        internal static extern void desktop_get_item_position([MarshalAs(UnmanagedType.LPWStr)] string path, out int x, out int y);

        [DllImport("desktop-lib.dll")]
        internal static extern void desktop_set_item_position([MarshalAs(UnmanagedType.LPWStr)] string path, int x, int y);
        #endregion Import
    }

    [Serializable]
    public sealed class DirectoryEx
    {
        #region Public Variables
        public event Action<string> OnNameChanged;
        public event Action<Vector2Int> OnPositionChanged;
        #endregion Public Variables

        #region Public Variables
        /* todo: sometimes after rename the file changes its position, why? */
        [ShowInInspector]
        public string Name
        {
            get => _Info.Name;
            set
            {
                _Info.MoveTo(Directory + "\\" + value);
                OnNameChanged?.Invoke(value);

                /* ensure that directory stays at the correct position */
                DesktopEx.desktop_set_item_position(Path, _Position.x, _Position.y);
                OnPositionChanged?.Invoke(_Position);
            }
        }

        [ShowInInspector]
        public string Path => _Info.FullName;

        [ShowInInspector]
        public string Directory => System.IO.Path.GetDirectoryName(_Info.FullName);

        [ShowInInspector]
        public Vector2Int Position
        {
            get
            {
                DesktopEx.desktop_get_item_position(Path, out var x, out var y);
                return new Vector2Int(x, y);
            }
            set
            {
                _Position = value;
                DesktopEx.desktop_set_item_position(Path, _Position.x, _Position.y);

                OnPositionChanged?.Invoke(value);
            }
        }

        [ShowInInspector]
        public List<FileInfo> Files
        {
            get
            {
                var result = new List<FileInfo>();
                var files = _Info.GetFiles();
                foreach (var file in files)
                {
                    result.Add(file);
                }

                return result;
            }
        }

        [ShowInInspector]
        public List<DirectoryInfo> Directories
        {
            get
            {
                var result = new List<DirectoryInfo>();
                var dirs = _Info.GetDirectories();
                foreach (var dir in dirs)
                {
                    result.Add(dir);
                }

                return result;
            }
        }
        #endregion Public Variables

        #region Public Methods
        public DirectoryEx(DirectoryInfo directoryInfo)
        {
            _Info = directoryInfo;
            _Position = Position;
        }
        #endregion Public Methods

        #region Private Variables
        private DirectoryInfo _Info;

        [ShowInInspector]
        private Vector2Int _Position;
        #endregion Private Variables
    }
}