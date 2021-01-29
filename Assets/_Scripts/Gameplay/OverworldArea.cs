namespace Mcpgnz.DesktopFramework
{
    using System;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using Zenject;

    public class OverworldArea : MonoBehaviour
    {
        #region Public Methods
        public void Load(Level level)
        {
            if (_Current != null) { Destroy(_Current); }

            _Current = Instantiate(level, _Root);
            _Container.Inject(_Current);
        }
        #endregion Public Methods

        #region Inspector Variables
        [SerializeField] private RectTransform _Root;
        [SerializeField] private GameObject Explorer;

        [SerializeField, ReadOnly] public Level _Current;
        #endregion Inspector Variables

        #region Private Types
        [Serializable]
        private sealed class Entry
        {
            public string Name;
            public Level Level;
        }
        #endregion Private Types

        #region Private Methods
        private void TeleportExplorerBy(float x, float y)
        {
            var pos = Coordinates.UnityToNormalized(Explorer.transform.position);
            Explorer.transform.position = Coordinates.NormalizedToUnity(pos + new Vector2(x, y));
        }
        #endregion Private Methods

        #region Private Variables
        [Inject] private DiContainer _Container;
        #endregion Private Variables
    }
}