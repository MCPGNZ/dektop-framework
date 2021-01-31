namespace Mcpgnz.DesktopFramework
{
    using UnityEngine;
    using Zenject;

    public class DestroySelfOnCollision : MonoBehaviour
    {
        [SerializeField] private bool _Pickable;
        private void OnCollisionEnter2D(Collision2D collision)
        {
            // make sure it only collides with the player
            if (collision.gameObject.GetComponent<Movement>() == null) { return; }

            if (_Pickable)
            {
                var actor = GetComponent<Actor>();
                if (actor != null) { actor.Cell.Discarded = true; }
            }

            Destroy(gameObject);
        }
    }
}