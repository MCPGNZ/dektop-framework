namespace Mcpgnz.DesktopFramework
{
    using UnityEngine;

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
            for (int i = 0; i < damage; i++)
            {
                Debug.Log("Ouch! It hurt!");
            }
        }

        [SerializeField] private int damage = 1;
    }
}