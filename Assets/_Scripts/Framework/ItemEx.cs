namespace Mcpgnz.DesktopFramework
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using Sirenix.OdinInspector;
    using UnityEngine;

    public interface IItemEx
    {
        #region Variables
        string Name { get; set; }
        string AbsolutePath { get; }
        string DirectoryPath { get; }

        Vector2Int Position { get; set; }
        #endregion Variables
    }

    public abstract class ItemEx : IItemEx
    {
        #region Public Variables
        public abstract string Name { get; set; }
        public abstract string AbsolutePath { get; }
        public abstract string DirectoryPath { get; }

        public abstract Vector2Int Position { get; set; }
        #endregion Public Variables

        #region Import
        [DllImport("desktop-lib.dll")]
        internal static extern void set_icon(
            [MarshalAs(UnmanagedType.LPWStr)] string itemPath,
            [MarshalAs(UnmanagedType.LPWStr)] string iconPath);

        [DllImport("desktop-lib.dll")]
        internal static extern void set_tooltip(
            [MarshalAs(UnmanagedType.LPWStr)] string itemPath,
            [MarshalAs(UnmanagedType.LPWStr)] string tooltip);
        #endregion Import
    }

    public class ItemEx<T> : ItemEx
        where T : FileSystemInfo
    {
        #region Public Variables
        public event Action<string> OnNameChanged;
        public event Action<Vector2Int> OnPositionChanged;

        [ShowInInspector]
        public override string Name
        {
            get => _Info.Name;
            set
            {
                if (_Info is DirectoryInfo directoryInfo) { directoryInfo.MoveTo(Path.Combine(DirectoryPath, value)); }
                if (_Info is FileInfo fileInfo) { fileInfo.MoveTo(Path.Combine(DirectoryPath, value)); }

                OnNameChanged?.Invoke(value);
            }
        }

        [ShowInInspector]
        public override string AbsolutePath => _Info.FullName;

        public override string DirectoryPath => Path.GetDirectoryName(_Info.FullName);

        public override Vector2Int Position
        {
            get
            {
                DesktopEx.desktop_get_item_position(AbsolutePath, out var x, out var y);
                return new Vector2Int(x, y);
            }
            set
            {
                DesktopEx.desktop_set_item_position(AbsolutePath, value.x, value.y);

                OnPositionChanged?.Invoke(value);
            }
        }

        public IconEx Icon
        {
            set => set_icon(AbsolutePath, value.AbsolutePath);
        }
        public string Tooltip
        {
            set => set_tooltip(AbsolutePath, value);
        }
        #endregion Public Variables

        #region Public Methods
        public ItemEx(T info)
        {
            _Info = info;
        }
        #endregion Public Methods

        #region Private Variables
        protected T _Info;
        #endregion Private Variables

    }

}