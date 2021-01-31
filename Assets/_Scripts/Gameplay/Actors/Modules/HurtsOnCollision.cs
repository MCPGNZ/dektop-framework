namespace Mcpgnz.DesktopFramework
{
    using UnityEngine;
    using Zenject;

    public class HurtsOnCollision : MonoBehaviour
    {
        #region Inspector Variables
        [SerializeField] private int _Damage = 1;
        #endregion Inspector Variables

        #region Unity Methods
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
        #endregion Unity Methods

        #region Private Variables
        [Inject] private Explorer _Explorer;
        #endregion Private Variables

        #region Private Methds
        private void OnAnyKindOfContact(Collider2D collider)
        {
            var bodyObject = collider.attachedRigidbody.gameObject;
            Debug.Log($"I collided with {bodyObject.name}.");

            // make sure it only hurts the player (who has Movement behavior)
            if (bodyObject.GetComponent<Movement>() == null) { return; }

            _Explorer.Lives -= _Damage;

            if (_Damage > 0)
            {
                // Debug.Log("Ouch! It hurt!");
                var reaction = bodyObject.GetComponent<DamageReaction>();
                if (reaction != null) reaction.OnDamage();
            }
        }
        #endregion Private Methds
    }
}