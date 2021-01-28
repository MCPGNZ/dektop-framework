namespace Mcpgnz.DesktopFramework
{
    using UnityEngine;
    using UnityRawInput;

    public sealed class Game : MonoBehaviour
    {
        #region Unity Methods
        public void Awake()
        {
            FrameworkEx.Initialize();

            StoreSettings();
            Setup();
        }
        private void OnDestroy()
        {
            RawKeyInput.Stop();
            FrameworkEx.Cleanup();

            RestoreSettings();
        }
        #endregion Unity Methods

        #region Private Variables
        private bool _AutoArrange;
        private bool _GridAlign;
        #endregion Private Variables

        #region Private Methods
        private void Setup()
        {
            RawKeyInput.Start(true);
            DesktopEx.Style(DesktopEx.FolderFlags.FWF_AUTOARRANGE, false);
            DesktopEx.Style(DesktopEx.FolderFlags.FWF_SNAPTOGRID, false);
            // don't minimize the editor
#if !UNITY_EDITOR
            Framework.GameWindow.Minimize();
#endif
        }

        private void StoreSettings()
        {
            _AutoArrange = DesktopEx.Style(DesktopEx.FolderFlags.FWF_AUTOARRANGE);
            _GridAlign = DesktopEx.Style(DesktopEx.FolderFlags.FWF_SNAPTOGRID);
        }
        private void RestoreSettings()
        {
            DesktopEx.Style(DesktopEx.FolderFlags.FWF_AUTOARRANGE, _AutoArrange);
            DesktopEx.Style(DesktopEx.FolderFlags.FWF_SNAPTOGRID, _GridAlign);
        }
        #endregion Private Methods
    }
}