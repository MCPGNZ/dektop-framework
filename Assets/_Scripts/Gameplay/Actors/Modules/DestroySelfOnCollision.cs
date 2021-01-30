namespace Mcpgnz.DesktopFramework
{
    using UnityEngine;
    using Zenject;

    public class DestroySelfOnCollision : MonoBehaviour
    {

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // make sure it only collides with the player
            if (collision.gameObject.GetComponent<Movement>() == null) { return; }

            Destroy(gameObject);
        }
    }
}