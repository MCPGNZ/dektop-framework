namespace Mcpgnz.DesktopFramework
{
    using UnityEngine;
    using Screen = System.Windows.Forms.Screen;

    public static class Coordinates
    {
        #region Public Methods
        public static Vector2 UnityToNormalized(Vector3 unityPosition)
        {
            return new Vector2(
                unityPosition.x / Config.UnitySize.x,
                -unityPosition.y / Config.UnitySize.y);
        }
        public static Vector3 NormalizedToUnity(Vector2 normalizedPosition)
        {
            return new Vector3(
                normalizedPosition.x * Config.UnitySize.x,
                -normalizedPosition.y * Config.UnitySize.y, 0.0f);
        }

        public static Vector3 DesktopToUnity(Vector2Int desktopPosition)
        {
            return NormalizedToUnity(DesktopToNormalized(desktopPosition));
        }
        public static Vector2Int UnityToDesktop(Vector3 unityPosition)
        {
            return NormalizedToDesktop(UnityToNormalized(unityPosition));
        }

        public static Vector2Int NormalizedToDesktop(Vector2 normalizedPosition)
        {
            var screen = Screen.PrimaryScreen;
            var size = screen.WorkingArea.Size;
            var offset = screen.Bounds.Location;

            return new Vector2Int(
                (int)(size.Width * normalizedPosition.x) - offset.X,
                (int)(size.Height * normalizedPosition.y) - offset.Y);
        }
        public static Vector2 DesktopToNormalized(Vector2Int desktopPosition)
        {
            var screen = Screen.PrimaryScreen;
            var size = screen.WorkingArea.Size;
            var offset = screen.Bounds.Location;

            return new Vector2(
                (desktopPosition.x + offset.X) / (float)size.Width,
                (desktopPosition.y + offset.Y) / (float)size.Height);
        }

        public static Vector2 GridToNormalized(Vector2Int position, Vector2Int gridSize)
        {
            return new Vector2(position.x / (float)(gridSize.x - 0), position.y / (float)(gridSize.y - 0));
        }
        public static Vector2Int NormalizedToGrid(Vector2 position, Vector2Int gridSize)
        {
            return new Vector2Int((int)(position.x * (gridSize.x + 0)), (int)(position.y * (gridSize.y + 0)));
        }
        #endregion Public Methods
    }
}