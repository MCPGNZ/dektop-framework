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
            RawKeyInput.Start(true);
        }
        private void OnDestroy()
        {
            RawKeyInput.Stop();
            FrameworkEx.Cleanup();
        }
        #endregion Unity Methods
    }
}