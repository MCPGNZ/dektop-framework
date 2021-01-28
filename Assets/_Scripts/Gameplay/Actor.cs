namespace Mcpgnz.DesktopFramework
{
    using Sirenix.OdinInspector;
    using UnityEngine;

    public sealed class Actor : MonoBehaviour
    {
        #region Unity Methods
        public void Start()
        {
            _Directory = DesktopEx.CreateDirectory(_Name);
            _Directory.Icon = _Icon;
            _Directory.Tooltip = _Tooltip;
            _Directory.Position = new Vector2Int(100, 100);
        }
        public void OnDestroy()
        {
            _Directory.Delete();
        }
        #endregion Unity Methods

        #region Inspector Variables
        [SerializeField] private string _Name;

        [SerializeField] private IconEx _Icon;
        [SerializeField] private string _Tooltip;
        #endregion Inspector Variables

        #region Private Variables
        [ShowInInspector, HideInEditorMode]
        private DirectoryEx _Directory;
        #endregion Private Variables
    }
}