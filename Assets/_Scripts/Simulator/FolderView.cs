namespace Mcpgnz.DesktopSimulator
{
    using Sirenix.OdinInspector;
    using TMPro;
    using UnityEngine;

    public sealed class FolderView : MonoBehaviour
    {
        #region Private Variables
        private bool IsBound => _Item != null;
        #endregion Private Variables

        #region Public Methods
        public void Bind(ItemEx item)
        {
            _Item = item;
            Refresh();
        }
        public void Unbind()
        {
            _Item = null;

            gameObject.name = "Folder [/unbound/]";
            _Name.SetText(string.Empty);
        }
        #endregion Public Methods

        #region Inspector Variables
        [SerializeField, BoxGroup("References")] private TMP_Text _Name;
        #endregion Inspector Variables

        #region Private Variables
        [ShowInInspector, BoxGroup("Preview")] private ItemEx _Item;
        #endregion Private Variables

        #region Unity Methods
        private void FixedUpdate()
        {
            if (IsBound == false) { return; }

            transform.position = Simulator.ToViewPosition(_Item);
        }
        #endregion Unity Methods

        #region Private Methods
        private void Refresh()
        {
            var name = _Item.Name;

            gameObject.name = $"Folder [{name}]";
            _Name.SetText(name);
        }
        #endregion Private Methods
    }
}