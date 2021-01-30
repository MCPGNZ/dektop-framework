namespace Mcpgnz.DesktopFramework
{
    using System;
    using Sirenix.OdinInspector;
    using UnityEngine;

    public sealed class Actor : MonoBehaviour
    {
        #region Public Variables
        public bool IsCreated => _Directory != null;
        public LevelParser.Cell Cell;
        public Vector2 NormalizedPosition
        {
            get => Coordinates.UnityToNormalized(transform.position);
            set => _UnityPosition = Coordinates.NormalizedToUnity(value);
        }
        #endregion Public Variables

        #region Public Methods
        public void Create(string name, LevelParser.Cell cell = null)
        {
            if (Config.DisableIcons) { return; }
            if (_Directory != null) { throw new InvalidOperationException(); }

            Cell = cell;
            _Name = name;
            _Directory = DesktopEx.CreateDirectory(_Name);
            _Directory.Icon = _Icon;
            _Directory.Tooltip = _Tooltip;
        }
        public void Destroy()
        {
            if (_Directory == null) { throw new InvalidOperationException(); }

            _Directory.Delete();
            _Directory = null;
        }
        public void ChangeIcon(IconEx newIcon)
        {
            if (_Directory == null) { throw new InvalidOperationException(); }

            _Directory.Icon = newIcon;
        }
        #endregion Public Methods

        #region Unity Methods
        private void Start()
        {
            if (_AutoCreate) { Create(_Name); }

            UpdatePosition();
        }
        private void FixedUpdate()
        {
            if (_Directory == null) { return; }
            UpdatePosition();
        }
        private void OnDestroy()
        {
            if (_Directory == null) { return; }

            Destroy();
        }
        #endregion Unity Methods

        #region Inspector Variables
        [SerializeField] private string _Name;

        [SerializeField] private IconEx _Icon;
        [SerializeField] private string _Tooltip;

        [SerializeField] private bool _AutoCreate;
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
                _Directory.DesktopPosition = Coordinates.UnityToDesktop(_UnityPosition);
            }
        }
        #endregion Private Methods
    }
}