namespace Mcpgnz.DesktopFramework
{
    using System;
    using Sirenix.OdinInspector;
    using UnityEngine;

    public sealed class Actor : MonoBehaviour, Overworld.IPooled
    {
        #region Public Variables
        public bool IsCreated => _Directory != null;
        public LevelParser.Cell Cell;
        public Vector2 NormalizedPosition
        {
            get => Coordinates.UnityToNormalized(transform.position);
            set => _UnityPosition = Coordinates.NormalizedToUnity(value);
        }

        public string Tooltip
        {
            set
            {
                _Tooltip = value;
                _Directory.Tooltip = value;
            }

        }
        #endregion Public Variables

        #region Public Methods
        public void Create(string uniqueId, LevelParser.Cell cell = null)
        {
            string name = uniqueId;

            // null for Explorer and HUD elements
            if (cell != null)
            {
                switch (cell.Type)
                {
                    case Identifier.PortalEntry:
                    {
                        name = $"PortalEntry{name}";
                        GetComponent<Actor>().GetComponent<PortalEntry>().PortalExitKey = cell.MatchingPortalExitKey;
                        break;
                    }

                    case Identifier.MineEnemy:
                    {
                        name = $"Minesweeper{name}.exe";
                        break;
                    }
                    case Identifier.BatEnemy:
                    {
                        name = $"Killer{name}.bat";
                        break;
                    }
                    default:
                    {
                        name = $"{cell.Type.ToName()}{name}";
                        break;
                    }
                }
            }

            if (Config.DisableIcons) { return; }
            if (_Directory != null) { throw new InvalidOperationException($"actor: {name}"); }

            Cell = cell;
            _Name = name;
            _Directory = DesktopEx.CreateDirectory(_Name);
            _Directory.Icon = _Icon;
            _Directory.Tooltip = _Tooltip;
        }
        public void Destroy()
        {
            if (_Directory == null) { throw new InvalidOperationException($"actor: {name}"); }

            _UnityPosition = Coordinates.NormalizedToUnity(new Vector2(-5, -5));
            UpdatePosition(true);

            _Directory.Delete();
            _Directory = null;
        }
        public void ChangeIcon(IconEx newIcon)
        {
            if (_Directory == null) { throw new InvalidOperationException($"actor: {name}"); }

            _Directory.Icon = newIcon;
        }

        void Overworld.IPooled.OnCreate()
        {
            UpdatePosition();
        }
        void Overworld.IPooled.OnRelease()
        {

            Destroy();
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
        public void UpdatePosition(bool force = false)
        {
            // Do not bother the backend with moving icons for micro-amounts.
            var distance = _UnityPosition - transform.localPosition;

            if (force || distance.magnitude > 1.0f)
            {
                _UnityPosition = transform.localPosition;
                _Directory.DesktopPosition = Coordinates.UnityToDesktop(_UnityPosition);

                /* now ! */
                if (force) { Lifetime.RefreshPositions(); }
            }
        }
        #endregion Private Methods
    }
}