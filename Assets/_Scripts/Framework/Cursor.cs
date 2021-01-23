namespace Mcpgnz.DesktopFramework
{
    using UnityEngine;
    using Vanara.PInvoke;

    public sealed class Cursor
    {
        #region Public Variables
        public static Vector2Int Position
        {
            get
            {
                User32.GetCursorPos(out var point);
                return new Vector2Int(point.X, point.Y);
            }
            set => User32.SetCursorPos(value.x, value.y);
        }
        #endregion Public Variables
    }
}