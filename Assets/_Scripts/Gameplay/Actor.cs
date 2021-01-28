namespace Mcpgnz.DesktopFramework
{
    using UnityEngine;

    public sealed class Actor : MonoBehaviour
    {
        #region Unity Methods
        public void Start()
        {
            _Directory = DesktopEx.CreateDirectory(_Name);

            _Icon[0].Tooltip(_Directory, _Tooltip);
            _Icon[0].Apply(_Directory);
        }
        public void OnDestroy()
        {
            _Directory.Delete();
        }

        public void FixedUpdate()
        {
            _Icon[_Count].Apply(_Directory);
            _Count = (_Count + 1) % _Icon.Length;
        }
        #endregion Unity Methods

        #region Inspector Variables
        [SerializeField] private string _Name;
        [SerializeField] private string _Tooltip;
        [SerializeField] private IconEx[] _Icon;
        #endregion Inspector Variables

        #region Private Variables
        private DirectoryEx _Directory;
        private int _Count;
        #endregion Private Variables
    }
}