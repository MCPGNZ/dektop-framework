namespace Mcpgnz.DesktopFramework
{
    using UnityEngine;
    using UnityRawInput;

    public sealed class Movement : MonoBehaviour
    {
        #region Inspector Variables
        [SerializeField] private float _Speed = 1.0f;
        [SerializeField] private RawKey _Up = RawKey.Up;
        [SerializeField] private RawKey _Down = RawKey.Down;
        [SerializeField] private RawKey _Right = RawKey.Right;
        [SerializeField] private RawKey _Left = RawKey.Left;
        #endregion Inspector Variables

        #region Unity Methods
        private void Start()
        {
            rigidbody2d = GetComponent<Rigidbody2D>();
        }
        private void FixedUpdate()
        {
            var direction = Vector2.zero;
            if (RawKeyInput.IsKeyDown(_Up)) { direction += new Vector2(0, 1); }
            if (RawKeyInput.IsKeyDown(_Down)) { direction += new Vector2(0, -1); }
            if (RawKeyInput.IsKeyDown(_Right)) { direction += new Vector2(1, 0); }
            if (RawKeyInput.IsKeyDown(_Left)) { direction += new Vector2(-1, 0); }

            var vector = direction.normalized * (_Speed * Time.deltaTime);
            Move(vector);
        }
        #endregion Unity Methods

        #region Private Methods
        private void Move(Vector2 vector)
        {
            if (rigidbody2d.bodyType == RigidbodyType2D.Kinematic)
            {
                rigidbody2d.MovePosition(rigidbody2d.position + vector);
            }
            else
            {
                rigidbody2d.AddForce(vector);
            }
        }
        #endregion Private Methods

        #region Private Variables
        Rigidbody2D rigidbody2d;
        #endregion
    }
}