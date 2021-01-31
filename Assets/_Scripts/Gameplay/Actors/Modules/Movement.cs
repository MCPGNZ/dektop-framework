namespace Mcpgnz.DesktopFramework
{
    using UnityEngine;
    using UnityRawInput;

    public sealed class Movement : MonoBehaviour
    {
        #region Inspector Variables
        [SerializeField] private float _Speed = 2000.0f;
        [SerializeField] private float _DashSpeed = 6000.0f;
        [SerializeField] private float _DashCooldown = 2.0f;
        [SerializeField] private float _DashDuration = 0.3f;
        [SerializeField] private RawKey _Up = RawKey.Up;
        [SerializeField] private RawKey _Down = RawKey.Down;
        [SerializeField] private RawKey _Right = RawKey.Right;
        [SerializeField] private RawKey _Left = RawKey.Left;
        [SerializeField] private RawKey _Dash = RawKey.Space;
        #endregion Inspector Variables

        #region Unity Methods
        private void Awake()
        {
            _Rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            var direction = Vector2.zero;
            if (RawKeyInput.IsKeyDown(_Up)) { direction += new Vector2(0, 1); }
            if (RawKeyInput.IsKeyDown(_Down)) { direction += new Vector2(0, -1); }
            if (RawKeyInput.IsKeyDown(_Right)) { direction += new Vector2(1, 0); }
            if (RawKeyInput.IsKeyDown(_Left)) { direction += new Vector2(-1, 0); }
            direction = direction.normalized;

            DashingUpdate();
            var isDashing = (_CurrentDashDuration > 0.0f);
            if (isDashing) { Move(direction * _DashSpeed * Time.fixedDeltaTime); }
            else { Move(direction * _Speed * Time.fixedDeltaTime); }
        }
        #endregion Unity Methods

        #region Private Variables
        private Rigidbody2D _Rigidbody;
        private float _CurrentDashCooldown = 0.0f;
        private float _CurrentDashDuration = 0.0f;
        #endregion Private Variables

        #region Private Methods
        private void Move(Vector2 vector)
        {
            if (_Rigidbody.bodyType == RigidbodyType2D.Kinematic)
            {
                _Rigidbody.MovePosition(_Rigidbody.position + vector);
            }
            else
            {
                _Rigidbody.AddForce(vector);
            }
        }
        private void DashingUpdate()
        {
            _CurrentDashCooldown -= Time.fixedDeltaTime;
            _CurrentDashDuration -= Time.fixedDeltaTime;

            if (_CurrentDashCooldown > 0) { return; }
            if (RawKeyInput.IsKeyDown(_Dash)) {
                _CurrentDashCooldown = _DashCooldown;
                _CurrentDashDuration = _DashDuration;
            }

        }
        #endregion Private Methods

    }
}