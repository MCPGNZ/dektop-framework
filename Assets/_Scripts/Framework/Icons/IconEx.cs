namespace Mcpgnz.DesktopFramework
{
    using System.IO;
    using Sirenix.OdinInspector;
    using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

    [CreateAssetMenu(menuName = "Framework/Icon"), HideMonoScript]
    public sealed class IconEx : ScriptableObject
    {
        #region Public Methods
        public string AbsolutePath => Path.Combine(Application.streamingAssetsPath, _Path);
        #endregion Public Methods

        #region Inspector Variables
        [SerializeField] private Object _Asset;
        [SerializeField, ReadOnly] private string _Path;
        #endregion Inspector Variables

        #region Unity Methods
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_Asset == null) { return; }

            /* todo: hardcoded */
            _Path = AssetDatabase.GetAssetPath(_Asset).Replace("Assets/StreamingAssets/", string.Empty);
        }
#endif
        #endregion Unity Methods

    }
}