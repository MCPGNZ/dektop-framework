namespace Mcpgnz.DesktopFramework
{
    using System;
    using UnityEngine;

    public sealed class Explorer : MonoBehaviour
    {
        #region Public Variables
        public Action<int> OnLifeLost;

        public int Lives
        {
            get => _Lives;
            set
            {
                if (value < _Lives) { OnLifeLost?.Invoke(_Lives - value); }

                _Lives = value;
            }
        }
        #endregion Public Variables

        #region Inspector Variables
        [SerializeField] private int _Lives = 3;
        #endregion Inspector Variables
    }
}