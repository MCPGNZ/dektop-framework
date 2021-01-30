namespace Mcpgnz.DesktopFramework
{
    using Sirenix.OdinInspector;
    using UnityEngine;
    using Zenject;

    public class PortalEntry : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            _Overworld.TeleportExplorerTo(PortalExitKey);
        }

        [Inject]
        private Overworld _Overworld;

        [ShowInInspector, HideInEditorMode]
        public string PortalExitKey;
    }
}