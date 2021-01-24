namespace Mcpgnz.DesktopSimulator
{
    using Sirenix.OdinInspector;
    using TMPro;
    using UnityEngine;

    public sealed class FolderView : MonoBehaviour
    {
        #region Private Variables
        public DirectoryEx Directory => _Directory;
        #endregion Private Variables

        #region Public Methods
        public void Bind(DirectoryEx directory)
        {
            _Directory = directory;

            _Directory.OnNameChanged += OnNameChanged;
            _Directory.OnPositionChanged += OnPositionChanged;

            OnNameChanged(_Directory.Name);
            OnPositionChanged(_Directory.Position);
        }

        public void Unbind()
        {
            _Directory.OnPositionChanged -= OnPositionChanged;
            _Directory = null;
        }
        #endregion Public Methods

        #region Inspector Variables
        [SerializeField, BoxGroup("References")] private TMP_Text _Name;
        #endregion Inspector Variables

        #region Private Variables
        [ShowInInspector, BoxGroup("Preview")] private DirectoryEx _Directory;
        #endregion Private Variables

        #region Private Methods
        private void OnNameChanged(string name)
        {
            gameObject.name = $"Folder [{name}]";
            _Name.SetText(name);
        }

        private void OnPositionChanged(Vector2Int obj)
        {
            transform.position = Simulator.ToViewPosition(_Directory);
        }
        #endregion Private Methods
    }
}