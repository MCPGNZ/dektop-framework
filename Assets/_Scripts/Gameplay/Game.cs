namespace Mcpgnz.DesktopFramework
{
    using UnityEngine;

    public sealed class Game : MonoBehaviour
    {
        #region Unity Methods
        public void Awake()
        {
            FrameworkEx.Initialize();
        }
        private void OnDestroy()
        {
            FrameworkEx.Cleanup();
        }
        #endregion Unity Methods
    }
}