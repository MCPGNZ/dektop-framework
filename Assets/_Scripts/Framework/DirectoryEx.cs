namespace Mcpgnz.DesktopFramework
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [Serializable]
    public sealed class DirectoryEx : IItemEx
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
                _Info.MoveTo(System.IO.Path.Combine(Directory, value));
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
        public List<FileEx> Files
        {
            get
            {
                var result = new List<FileEx>();
                var files = _Info.GetFiles();
                foreach (var file in files)
                {
                    result.Add(new FileEx(file));
                }

                return result;
            }
        }

        [ShowInInspector]
        public List<DirectoryEx> Directories
        {
            get
            {
                var result = new List<DirectoryEx>();
                var dirs = _Info.GetDirectories();
                foreach (var dir in dirs)
                {
                    result.Add(new DirectoryEx(dir));
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

        [Button]
        public DirectoryEx CreateDirectory(string name)
        {
            return new DirectoryEx(_Info.CreateSubdirectory(name));
        }

        [Button]
        public void Delete()
        {
            _Info.SafeDelete();
            _Info = null;
        }
        #endregion Public Methods

        #region Private Variables
        private DirectoryInfo _Info;
        private Vector2Int _Position;
        #endregion Private Variables
    }
}