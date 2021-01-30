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

            Debug.Log("Ouch! It hurt!");
            _Controller.Lifepoints -= _Damage;

            var reaction = collision.gameObject.GetComponent<DamageReaction>();
            if (reaction != null) reaction.OnDamage();
        }

        [SerializeField] private int _Damage = 1;
        [Inject] private HUDController _Controller;
    }
}