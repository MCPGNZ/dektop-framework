namespace Mcpgnz.DesktopFramework
{
    using UnityEngine;

    public sealed class Explorer : MonoBehaviour
    {
        #region Public Variables
        public int Lives
        {
            get => _Lives;
            set => _Lives = value;
        }
        #endregion Public Variables

        #region Inspector Variables
        [SerializeField] private int _Lives = 3;
        #endregion Inspector Variables
    }
}