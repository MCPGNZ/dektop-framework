namespace Mcpgnz.DesktopFramework
{
    using System.IO;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public interface IItemEx
    {
        #region Variables
        string Name { get; set; }
        string AbsolutePath { get; }
        string DirectoryPath { get; }

        Vector2Int DesktopPosition { get; set; }
        #endregion Variables
    }

    public abstract class ItemEx : IItemEx
    {
        #region Public Variables
        public abstract bool IsCreated { get; }
        public abstract string Name { get; set; }
        public abstract string AbsolutePath { get; }
        public abstract string DirectoryPath { get; }

        public abstract Vector2Int DesktopPosition { get; set; }
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

        public Vector2Int _Position;
    }

    public class ItemEx<T> : ItemEx
        where T : FileSystemInfo
    {
        #region Public Variables
        public override bool IsCreated => _Info != null;

        public override string Name
        {
            get => _Info.Name;
            set
            {
                if (_Info is DirectoryInfo directoryInfo) { directoryInfo.MoveTo(Path.Combine(DirectoryPath, value)); }
                if (_Info is FileInfo fileInfo) { fileInfo.MoveTo(Path.Combine(DirectoryPath, value)); }
            }
        }

        public override string AbsolutePath => _Info.FullName;
        public override string DirectoryPath => Path.GetDirectoryName(_Info.FullName);

        public override Vector2Int DesktopPosition
        {
            get
            {
                DesktopEx.desktop_get_item_position(_Info.Name, out var x, out var y);
                return new Vector2Int(x, y);
            }
            set
            {

                _Position = value;

                if (Lifetime.UpdateList.Contains(this)) { return; }
                Lifetime.UpdateList.Add(this);
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