namespace Mcpgnz.DesktopFramework
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;

    public class Automobile : MonoBehaviour
    {

        public Vector2 MoveVector
        {
            get => _CurrentMoveVector;
            set => _CurrentMoveVector = value;
        }

        // Use this for initialization
        void Start()
        {
            rigidbody2d = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            // Change the Move Vector only once per simulation frame.
            if (_QueueHorizontalBounce) { _CurrentMoveVector *= new Vector2(-1.0f, 1.0f); }
            if (_QueueVerticalBounce) { _CurrentMoveVector *= new Vector2(1.0f, -1.0f); }
            _QueueHorizontalBounce = _QueueVerticalBounce = false;

            var vector = _CurrentMoveVector * Time.deltaTime;
            rigidbody2d.MovePosition(rigidbody2d.position + vector);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var point = collision.GetContact(0);
            var relativePos = point.normal;
            Debug.Log(String.Format($"Bouncing off from {collision.rigidbody.name} with {relativePos.x}, {relativePos.y}"));
            if (Math.Abs(relativePos.x) > Math.Abs(relativePos.y))
            {
                _QueueHorizontalBounce = true;
            }
            else
            {
                _QueueVerticalBounce = true;
            }
        }


        [SerializeField] private Vector2 _CurrentMoveVector = new Vector2(1.0f, 1.0f);

        #region Private Variables
        private Rigidbody2D rigidbody2d;
        private bool _QueueVerticalBounce = false;
        private bool _QueueHorizontalBounce = false;
        #endregion
    }
}