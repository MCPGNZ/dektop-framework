namespace Mcpgnz.DesktopFramework
{
    using UnityEngine;
    using System;

    public class Automobile : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            rigidbody2d = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            var vector = _CurrentMoveVector * Time.deltaTime;
            rigidbody2d.MovePosition(rigidbody2d.position + vector);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Will do a diagonal bounce off any collisions.
            var relativePos = collision.gameObject.transform.position - gameObject.transform.position;
            if (Math.Abs(relativePos.x) > Math.Abs(relativePos.y))
            {
                _CurrentMoveVector *= new Vector2(-1.0f, 1.0f);
            }
            else
            {
                _CurrentMoveVector *= new Vector2(1.0f, -1.0f);
            }
        }

        [SerializeField] private Vector2 _CurrentMoveVector = new Vector2(1.0f, 1.0f);

        #region Private Variables
        private Rigidbody2D rigidbody2d;
        #endregion
    }
}