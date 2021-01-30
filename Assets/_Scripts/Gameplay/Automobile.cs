namespace Mcpgnz.DesktopFramework
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;

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
            CheckCollisions();
            var vector = _CurrentMoveVector * Time.deltaTime;
            rigidbody2d.MovePosition(rigidbody2d.position + vector);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            
            // Store all collisions for later, but only the relative positions.
            if (collision.attachedRigidbody != null)
            {
                var relativePos = collision.attachedRigidbody.transform.position - gameObject.transform.position;
                Debug.Log(String.Format($"Bouncing off from {collision.attachedRigidbody.name} with {relativePos.x}, {relativePos.y}"));
                _SimultaneusCollisions.Add(relativePos);
            }
        }
        private void CheckCollisions()
        {
            if (_SimultaneusCollisions.Count < 1) return;
            // Will do a diagonal bounce off any collisions.
            var relativePos = GetBestCollider();
            if (Math.Abs(relativePos.x) > Math.Abs(relativePos.y))
            {
                _CurrentMoveVector *= new Vector2(-1.0f, 1.0f);
            }
            else
            {
                _CurrentMoveVector *= new Vector2(1.0f, -1.0f);
            }
        }
        private Vector3 GetBestCollider()
        {
            // If you hit multiple objects at once, only bounce off the closest.
            Vector3 best = _SimultaneusCollisions[0];
            foreach (var item in _SimultaneusCollisions)
            {
                if (item.magnitude > best.magnitude) best = item;
            }
            _SimultaneusCollisions.Clear();
            return best;
        }

        [SerializeField] private Vector2 _CurrentMoveVector = new Vector2(1.0f, 1.0f);

        #region Private Variables
        private Rigidbody2D rigidbody2d;
        private List<Vector3> _SimultaneusCollisions = new List<Vector3>();
        #endregion
    }
}