namespace Mcpgnz.DesktopFramework
{
    using Sirenix.OdinInspector;
    using UnityEngine;
    using Zenject;

    public class PortalEntry : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            _Overworld.TeleportExplorerTo(PortalExitKey);
        }

        [ShowInInspector, HideInEditorMode]
        public string PortalExitKey;

        [Inject]
        private Overworld _Overworld;
    }
}