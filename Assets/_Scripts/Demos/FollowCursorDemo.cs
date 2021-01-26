namespace Mcpgnz.DesktopFramework.Demos
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityRawInput;

    public static class Extensions
    {
        public static Vector2 ToVec(this MousePosition pos)
        {
            return new Vector2 { x = pos.x, y = pos.y };
        }

        public static Vector2Int ToIntVec(this Vector2 pos)
        {
            return new Vector2Int { x = (int)pos.x, y = (int)pos.y };
        }
    }

    public sealed class FollowCursorDemo : MonoBehaviour
    {
        #region Inspector Variables
        [SerializeField] private int _NumIcons = 16;
        #endregion Inspector Variables

        #region Unity Methods
        public void Awake()
        {
            RawMouseInput.Start(workInBackground: true);
            RawMouseInput.OnMouseMove += OnMouseMove;
            RawMouseInput.OnMouseLeftDown += OnMouseLeftDown;

            FrameworkEx.Initialize();

            for (int i = 0; i < _NumIcons; ++i)
            {
                _Representation.Add(DesktopEx.CreateDirectory($"tail[{i}]"));
            }
        }

        public void FixedUpdate()
        {
            if (_FollowCursor)
            {
                for (int i = 0; i < _Representation.Count; ++i)
                {
                    IItemEx dir = _Representation[i];
                    Vector2 pos = dir.Position;
                    Vector2 target = Vector2.Lerp(pos, _CursorPos, 0.5f + (float)i / (float)(_Representation.Count * 2));
                    dir.Position = target.ToIntVec();
                }
            }
        }

        public void OnDestroy()
        {
            RawMouseInput.Stop();
            RawMouseInput.OnMouseMove -= OnMouseMove;
            RawMouseInput.OnMouseLeftDown -= OnMouseLeftDown;

            foreach (var directory in _Representation)
            {
                directory.Delete();
            }

            FrameworkEx.Cleanup();
        }
        #endregion Unity Methods

        #region Private Variables
        private bool _FollowCursor = false;
        private Vector2 _CursorPos = new Vector2Int();
        private readonly List<DirectoryEx> _Representation = new List<DirectoryEx>();
        #endregion Private Variables

        #region Private Methods
        private void OnMouseMove(MousePosition pos)
        {
            _CursorPos = pos.ToVec();
        }

        private void OnMouseLeftDown(MousePosition pos)
        {
            _FollowCursor = !_FollowCursor;
        }
        #endregion Private Methods
    }
}
