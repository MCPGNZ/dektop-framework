namespace Mcpgnz.DesktopFramework
{
    using UnityEngine;
    using Zenject;

    public class DestroySelfOnCollision : MonoBehaviour
    {

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // This is for mines, who have solid bodies.
            OnAnyKindOfContact(collision.collider);
        }
        private void OnTriggerEnter2D(Collider2D collider)
        {
            // This is for spikes, who you can pass through.
            OnAnyKindOfContact(collider);
        }

        private void OnAnyKindOfContact(Collider2D collider)
        {
            var bodyObject = collider.attachedRigidbody.gameObject;
            Debug.Log($"I collided with {bodyObject.name}.");

            // make sure it only hurts the player (who has Movement behavior)
            if (bodyObject.GetComponent<Movement>() == null) { return; }

            Destroy(gameObject);
        }

    }
}