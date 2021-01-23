namespace Mcpgnz.DesktopSimulator
{
    using Mcpgnz.Utilities;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Mcpngz/Desktop/Config")]
    public sealed class Config : ScriptableObjectSingleton<Config>
    {
        #region Public Variables
        public static FolderView Folder => Instance._Folder;
        #endregion Public Variables

        #region Inspector Variables
        [SerializeField] private FolderView _Folder;
        #endregion Inspector Variables
    }
}