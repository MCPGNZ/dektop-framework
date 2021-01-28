namespace Mcpgnz.DesktopFramework
{
    using UnityEngine;

    public static class Coordinates
    {

        #region Public Types
        #endregion Public Types

        #region Public Methods
        public static Vector2 UnityToNormalized(Vector3 unityPosition)
        {
            return new Vector2(
                unityPosition.x / _Canvas.x,
                -unityPosition.y / _Canvas.y);
        }
        public static Vector3 NormalizedToUnity(Vector2 normalizedPosition)
        {
            return new Vector3(
                normalizedPosition.x * _Canvas.x,
                -normalizedPosition.y * _Canvas.y, 0.0f);
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
            return new Vector2Int(
               (int)(Screen.currentResolution.width * normalizedPosition.x),
                (int)(Screen.currentResolution.height * normalizedPosition.y));
        }
        public static Vector2 DesktopToNormalized(Vector2Int desktopPosition)
        {
            return new Vector2(
                desktopPosition.x / (float)Screen.currentResolution.width,
                desktopPosition.y / (float)Screen.currentResolution.height);
        }

        public static Vector2 GridToNormalized(Vector2Int position, Vector2Int gridSize)
        {
            return new Vector2(position.x / (float)gridSize.x, position.y / (float)gridSize.y);
        }
        public static Vector2Int NormalizedToGrid(Vector2 position, Vector2Int gridSize)
        {
            return new Vector2Int((int)(position.x * gridSize.x), (int)(position.y * gridSize.y));
        }
        #endregion Public Methods

        #region Private Variables
        private static readonly Vector2 _Canvas = new Vector2(1.6f, 0.9f);
        #endregion Private Variables
    }
}