namespace Mcpgnz.DesktopFramework
{
    using System;
    using System.IO;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [Serializable]
    public sealed class FileEx : IItemEx
    {
        #region Public Variables
        public event Action<string> OnNameChanged;
        public event Action<Vector2Int> OnPositionChanged;
        #endregion Public Variables

        #region Public Variables
        [ShowInInspector]
        public string Name
        {
            get => _Info.Name;
            set => _Info.MoveTo(value);
        }

        [ShowInInspector]
        public string Path => _Info.FullName;

        [ShowInInspector]
        public string Directory => _Info.Directory.FullName;

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
                DesktopEx.SetItemPosition(Path, value.x, value.y);

                OnPositionChanged?.Invoke(value);
            }
        }
        #endregion Public Variables

        #region Public Methods
        public FileEx(FileInfo directoryInfo)
        {
            _Info = directoryInfo;
        }
        #endregion Public Methods

        #region Public Variables
        private FileInfo _Info;
        #endregion Public Variables
    }
}