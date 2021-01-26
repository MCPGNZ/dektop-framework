// ReSharper disable All
namespace Mcpgnz.DesktopSimulator
{
    using Sirenix.OdinInspector;
    using System.Collections.Generic;
    using Mcpgnz.DesktopFramework;
    using UnityEngine;
    using UnityRawInput;

    public sealed class Simulator : MonoBehaviour
    {
        #region Public Methods
        public static Vector3 ToViewPosition(IItemEx item)
        {
            var position = item.Position;

            /* todo: proper conversion */
            return new Vector3(position.x, 1440 - position.y, 0.0f);
        }
        #endregion Public Methods

        #region Inspector Variables
        [SerializeField, BoxGroup("References")] private Canvas _ViewRoot;
        #endregion Inspector Variables

        #region Unity Methods
        private void Awake()
        {
            FrameworkEx.Initialize();
            Application.runInBackground = true;
            InvokeRepeating("LogCursorPos", 0.0f, 1.0f);

        }
        private void Start()
        {
            CreateView();
        }
        private void OnDestroy()
        {
            FrameworkEx.Cleanup();
        }

        private void OnEnable()
        {
            bool workInBackground = true;

            RawKeyInput.Start(workInBackground);
            RawKeyInput.OnKeyUp += OnKeyUp;
            RawKeyInput.OnKeyDown += OnKeyDown;

            RawMouseInput.Start(!workInBackground);
            RawMouseInput.OnMouseMove += OnMouseMove;
            RawMouseInput.OnMouseLeftDown += OnMouseLeftDown;
            RawMouseInput.OnMouseLeftUp += OnMouseLeftUp;
            RawMouseInput.OnMouseRightDown += OnMouseRightDown;
            RawMouseInput.OnMouseRightUp += OnMouseRightUp;
            RawMouseInput.OnMouseWheel += OnMouseWheel;
        }

        private void OnDisable()
        {
            RawKeyInput.Stop();
            RawKeyInput.OnKeyUp -= OnKeyUp;
            RawKeyInput.OnKeyDown -= OnKeyDown;

            RawMouseInput.Stop();
            RawMouseInput.OnMouseMove -= OnMouseMove;
            RawMouseInput.OnMouseLeftDown -= OnMouseLeftDown;
            RawMouseInput.OnMouseLeftUp -= OnMouseLeftUp;
            RawMouseInput.OnMouseRightDown -= OnMouseRightDown;
            RawMouseInput.OnMouseRightUp -= OnMouseRightUp;
            RawMouseInput.OnMouseWheel -= OnMouseWheel;
        }

        #endregion Unity Methods

        #region Private Variables
        private readonly List<FolderView> _FolderViews = new List<FolderView>();

        private MousePosition cursorPos;
        #endregion Private Variables

        #region Private Methods
        private void CreateView()
        {
            /* cleanup */
            DestroyView();

            /* build */
            var items = DesktopEx.Directories;
            var folderPrefab = Config.Folder;

            foreach (var item in items)
            {
                var view = Instantiate(folderPrefab, _ViewRoot.transform);
                view.Bind(item);

                _FolderViews.Add(view);
            }

        }
        private void DestroyView()
        {
            foreach (var folderView in _FolderViews)
            {
                folderView.Unbind();
                Destroy(folderView.gameObject);
            }
            _FolderViews.Clear();
        }

        private void OnKeyUp(RawKey key)
        {
            Debug.Log($"{key} up");
        }

        private void OnKeyDown(RawKey key)
        {
            Debug.Log($"{key} down");
        }

        private void LogCursorPos()
        {
            Debug.Log($"cursor: {cursorPos.x}, {cursorPos.y}");
        }

        private void OnMouseMove(MousePosition pos)
        {
            cursorPos = pos;
        }

        private void OnMouseLeftDown(MousePosition pos)
        {
            Debug.Log($"mouse left down {pos.x}, {pos.y}");
        }

        private void OnMouseLeftUp(MousePosition pos)
        {
            Debug.Log($"mouse left up {pos.x}, {pos.y}");
        }

        private void OnMouseRightDown(MousePosition pos)
        {
            Debug.Log($"mouse right down {pos.x}, {pos.y}");
        }

        private void OnMouseRightUp(MousePosition pos)
        {
            Debug.Log($"mouse right up {pos.x}, {pos.y}");
        }

        private void OnMouseWheel(MousePosition pos, int delta)
        {
            Debug.Log($"mouse wheel: {delta} at {pos.x}, {pos.y}");
        }
        #endregion Private Methods

    }
}