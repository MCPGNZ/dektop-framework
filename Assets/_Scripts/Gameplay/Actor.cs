namespace Mcpgnz.DesktopFramework
{
    using Sirenix.OdinInspector;
    using UnityEngine;

    public sealed class Actor : MonoBehaviour
    {
        #region Public Variables
        public Vector2 NormalizedPosition
        {
            get => Coordinates.UnityToNormalized(transform.position);
            set => _UnityPosition = Coordinates.NormalizedToUnity(value);
        }
        #endregion Public Variables

        #region Unity Methods
        public void Start()
        {
            _Directory = DesktopEx.CreateDirectory(_Name);
            _Directory.Icon = _Icon;
            _Directory.Tooltip = _Tooltip;

            UpdatePosition();
        }
        public void FixedUpdate()
        {
            UpdatePosition();
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

        private Vector3 _UnityPosition;
        #endregion Private Variables

        #region Private Methods
        private void UpdatePosition()
        {
            if (_UnityPosition != transform.localPosition)
            {
                _UnityPosition = transform.localPosition;
                _Directory.Position = Coordinates.UnityToDesktop(_UnityPosition);
            }
        }
        #endregion Private Methods
    }
}