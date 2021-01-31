namespace Mcpgnz.DesktopFramework
{
    using System;
    using System.IO;

    [Serializable]
    public sealed class FileEx : ItemEx<FileInfo>
    {
        #region Public Methods
        public FileEx(FileInfo fileIno) : base(fileIno)
        {
            _Info = fileIno;
        }

        public void Delete()
        {
            File.Delete(AbsolutePath);
        }
        #endregion Public Methods
    }
}