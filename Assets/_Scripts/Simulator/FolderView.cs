namespace Mcpgnz.DesktopSimulator
{
    using Mcpgnz.DesktopFramework;
    using Sirenix.OdinInspector;
    using TMPro;
    using UnityEngine;

    public sealed class FolderView : MonoBehaviour
    {
        #region Private Variables
        public IItemEx Item => _Item;
        #endregion Private Variables

        #region Public Methods
        public void Bind(IItemEx item)
        {
            _Item = item;

            _Item.OnNameChanged += OnNameChanged;
            _Item.OnPositionChanged += OnPositionChanged;

            OnNameChanged(_Item.Name);
            OnPositionChanged(_Item.Position);
        }

        public void Unbind()
        {
            _Item.OnPositionChanged -= OnPositionChanged;
            _Item = null;
        }
        #endregion Public Methods

        #region Inspector Variables
        [SerializeField, BoxGroup("References")]
        private TMP_Text _Name;
        #endregion Inspector Variables

        #region Private Variables
        [ShowInInspector, BoxGroup("Preview")]
        private IItemEx _Item;
        #endregion Private Variables

        #region Private Methods
        private void OnNameChanged(string name)
        {
            gameObject.name = $"Folder [{name}]";
            _Name.SetText(name);
        }

        private void OnPositionChanged(Vector2Int obj)
        {
            transform.position = Simulator.ToViewPosition(_Item);
        }
        #endregion Private Methods
    }
}