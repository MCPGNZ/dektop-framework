namespace Mcpgnz.DesktopFramework
{
    using Sirenix.OdinInspector;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Zenject;

    public class PortalEntry : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            _Overworld.TeleportExplorerTo(PortalExitKey);
        }

        [Inject]
        private Overworld _Overworld;

        [ShowInInspector, HideInEditorMode]
        public string PortalExitKey;
    }
}
