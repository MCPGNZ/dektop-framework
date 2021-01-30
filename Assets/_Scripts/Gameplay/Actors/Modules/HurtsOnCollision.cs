namespace Mcpgnz.DesktopFramework
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
            // make sure it only hurts the player (who has Movement behavior)
            if (collision.gameObject.GetComponent<Movement>() == null) { return; }

            _Controller.Lifepoints -= _Damage;

            if (_Damage > 0)
            {
                Debug.Log("Ouch! It hurt!");
                // TODO replace with Ailish sounds
                Sounds.WindowsHardwareRemove.Play();
                var reaction = collision.gameObject.GetComponent<DamageReaction>();
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