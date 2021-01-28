namespace Mcpgnz.DesktopFramework
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Sirenix.OdinInspector;

    [Serializable]
    public sealed class DirectoryEx : ItemEx<DirectoryInfo>
    {
        #region Public Variables
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
        public DirectoryEx(DirectoryInfo directoryInfo) : base(directoryInfo)
        {

        }

        public FileEx CreateFile(string name)
        {
            var fileInfo = new FileInfo(Path.Combine(AbsolutePath, name));
            using (var stream = fileInfo.Create()) { }

            return new FileEx(fileInfo);
        }
        public DirectoryEx CreateDirectory(string name)
        {
            return new DirectoryEx(_Info.CreateSubdirectory(name));
        }

        public void Delete()
        {
            _Info.SafeDelete();
            _Info = null;
        }
        #endregion Public Methods
    }
}