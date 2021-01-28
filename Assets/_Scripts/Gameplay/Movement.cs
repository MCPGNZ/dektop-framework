namespace Mcpgnz.DesktopFramework
{
    using UnityEngine;
    using UnityRawInput;

    public sealed class Movement : MonoBehaviour
    {
        #region Inspector Variables
        [SerializeField] private RawKey _Up = RawKey.Up;
        [SerializeField] private RawKey _Down = RawKey.Down;
        [SerializeField] private RawKey _Right = RawKey.Right;
        [SerializeField] private RawKey _Left = RawKey.Left;
        #endregion Inspector Variables

        #region Unity Methods
        private void Update()
        {
            var direction = Vector2.zero;
            if (RawKeyInput.IsKeyDown(_Up)) { direction += new Vector2(0, 1); }
            if (RawKeyInput.IsKeyDown(_Down)) { direction += new Vector2(0, -1); }
            if (RawKeyInput.IsKeyDown(_Right)) { direction += new Vector2(1, 0); }
            if (RawKeyInput.IsKeyDown(_Left)) { direction += new Vector2(-1, 0); }

            var vector = direction.normalized * (Config.MovementSpeed * Time.deltaTime);
            Move(vector);
        }
        #endregion Unity Methods

        #region Private Methods
        private void Move(Vector3 vector)
        {
            transform.position += vector;
        }
        #endregion Private Methods
    }
}