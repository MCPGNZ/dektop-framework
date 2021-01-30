﻿namespace Mcpgnz.DesktopFramework
{
    using UnityEngine;
    using Zenject;

    public class HurtsOnCollision : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

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

            _Controller.Lifepoints -= _Damage;

            if (_Damage > 0)
            {
                Debug.Log("Ouch! It hurt!");
                // TODO replace with Ailish sounds
                Sounds.WindowsHardwareRemove.Play();
		var reaction = bodyObject.GetComponent<DamageReaction>();
		if (reaction != null) reaction.OnDamage();
            }
            else if (_Damage < 0)
            {
                // TODO replace with Ailish sounds
                Sounds.WindowsHardwareInsert.Play();
            }
        }

        [SerializeField] private int _Damage = 1;
        [Inject] private HUDController _Controller;
    }
}
