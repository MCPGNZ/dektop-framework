namespace Mcpgnz.DesktopFramework
{
    using UnityEngine;

    public sealed class Actor : MonoBehaviour
    {
        #region Unity Methods
        public void Awake()
        {
            _Folder = DesktopEx.CreateDirectory(_Name);
            _Icon.Apply(_Folder);
        }
        public void OnDestroy()
        {
            _Folder.Delete();
        }
        #endregion Unity Methods

        #region Inspector Variables
        [SerializeField] private string _Name;
        [SerializeField] private IconEx _Icon;
        #endregion Inspector Variables

        #region Private Variables
        private DirectoryEx _Folder;
        #endregion Private Variables
    }
}