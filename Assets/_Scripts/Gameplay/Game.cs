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
            DesktopEx.AutoArrange = false;
            DesktopEx.GridAlign = false;
        }

        private void StoreSettings()
        {
            _AutoArrange = DesktopEx.AutoArrange;
            _GridAlign = DesktopEx.GridAlign;
        }
        private void RestoreSettings()
        {
            DesktopEx.AutoArrange = _AutoArrange;
            DesktopEx.GridAlign = _GridAlign;
        }
        #endregion Private Methods
    }
}